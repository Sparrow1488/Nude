using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Nude.API.Infrastructure.Clients.Telegraph;
using Nude.API.Infrastructure.Constants;
using Nude.Data.Infrastructure.Contexts;
using Nude.API.Infrastructure.Constants.Defaults;
using Nude.API.Infrastructure.Conventions;
using Nude.API.Infrastructure.Extensions;
using Nude.API.Infrastructure.Initializers;
using Nude.API.Infrastructure.Managers;
using Nude.API.Infrastructure.Middlewares;
using Nude.API.Infrastructure.Services.Keys;
using Nude.API.Infrastructure.Services.Randomizers;
using Nude.API.Infrastructure.Services.Resolvers;
using Nude.API.Infrastructure.Services.Storages;
using Nude.API.Services.Collections;
using Nude.API.Services.Formatters;
using Nude.API.Services.Images;
using Nude.API.Services.Limits;
using Nude.API.Services.Limits.Handlers;
using Nude.API.Services.Mangas;
using Nude.API.Services.Notifications;
using Nude.API.Services.Queues;
using Nude.API.Services.Stealers;
using Nude.API.Services.Tickets;
using Nude.API.Services.Users;
using Nude.API.Services.WebHooks;
using Nude.API.Services.Workers;
using Nude.Authorization.Handlers;
using Nude.Authorization.Stores;
using Nude.Creators;
using Nude.Mapping.Profiles;
using Nude.Mapping.Utils;
using Nude.Parsers.HentaiChan;
using Nude.Parsers.NudeMoon;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets(typeof(Program).Assembly, false);

#region Logger

var config = new LoggerConfiguration().WriteTo.Console();

if (builder.Environment.IsProduction())
{
    config.WriteTo.File("./log.txt");
}

Log.Logger = config.CreateLogger();
builder.Host.UseSerilog(Log.Logger);

#endregion

#region Controllers

builder.Services
    .AddControllers(opt =>
    {
        opt.Conventions.Add(new RoutePrefixConvention(new RouteAttribute(ApiDefaults.CurrentVersion)));
    })
    .AddNewtonsoftJson(options => options.BindOptions());

#endregion

#region Authentication & Authorization

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var rsa = RSA.Create();
        var key = KeysProvider.GetPrivateKey();
        rsa.ImportRSAPrivateKey(key, out _);

        options.Configuration = new OpenIdConnectConfiguration
        {
            SigningKeys =
            {
                new RsaSecurityKey(rsa)
            }
        };
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };

        options.MapInboundClaims = false;
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy(Policies.Admin, configure =>
    {
        configure.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        configure.RequireClaim(NudeClaimTypes.Role, NudeClaims.Roles.Administrator);
    });
});

#endregion

#region Services

builder.Services.AddScoped<IParserCreator, ParserCreator>();

builder.Services.AddScoped<IAuthHandler<INudeParser>, NudeMoonAuthHandler>();
builder.Services.AddScoped<IAuthHandler<IHentaiChanParser>, HentaiChanAuthHandler>();

builder.Services.AddScoped<ICredentialsSecureStore, CredentialsSecureStore>();

builder.Services.AddScoped<IMangaService, MangaService>();
builder.Services.AddScoped<IImagesService, ImagesService>();
builder.Services.AddScoped<IContentTicketService, ContentTicketService>();
builder.Services.AddScoped<IImageCollectionsService, ImageCollectionsService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserSession, UserSession>();
builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<IFileStorage, FileLocalStorage>();

builder.Services.AddScoped<ITagManager, TagManager>();
builder.Services.AddScoped<IMangaParserResolver, MangaParserResolver>();
builder.Services.AddScoped<IContentStealerService, ContentStealerService>();
builder.Services.AddScoped<IFormatterService, FormatterService>();

builder.Services.AddScoped<IFormatQueue, FormatQueue>();

builder.Services.AddScoped<IWebHookService, WebHookService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<ITelegraphClient, DefaultTelegraphClient>();

builder.Services.AddScoped<IRandomizer, CryptoRandomizer>();

builder.Services.AddScoped<ILimitService, LimitService>();
builder.Services.AddScoped<LimitHandler, ContentTicketCreationLimitHandler>();

builder.Services.AddHttpClient("web_hook", config =>
{
    config.Timeout = TimeSpan.FromSeconds(10);
});

#endregion

#region Database

var connectionString = builder.Configuration.GetConnectionString("Database");
var configureAction = new Action<DbContextOptionsBuilder>(
    x => x.UseNpgsql(
        connectionString, 
        b => b.MigrationsAssembly("Nude.API")));

builder.Services.AddDbContextFactory<AppDbContext>(configureAction);
builder.Services.AddDbContext<AppDbContext>(configureAction);

#endregion

#region Mappers

var profilesAssembly = typeof(MangaProfile).Assembly;
MapperInitializer.AssertConfigurationIsValid(profilesAssembly);
builder.Services.AddAutoMapper(x => x.AddMaps(profilesAssembly));

#endregion

#region Background Service

builder.Services.AddBackgroundWorkers(typeof(ContentTicketsWorker), typeof(FormatsWorker));

#endregion

var app = builder.Build();

if (app.Environment.IsProduction())
{
    await DatabaseInitializer.InitializeAsync<AppDbContext>(app.Services);
}

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorsMiddleware>();

app.MapControllers();

app.Run();