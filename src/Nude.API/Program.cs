using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nude.API.Infrastructure.Clients.Telegraph;
using Nude.Data.Infrastructure.Contexts;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Extensions;
using Nude.API.Infrastructure.Managers;
using Nude.API.Infrastructure.Middlewares;
using Nude.API.Infrastructure.Services.Resolvers;
using Nude.API.Services.Formatters;
using Nude.API.Services.Images;
using Nude.API.Services.Mangas;
using Nude.API.Services.Notifications;
using Nude.API.Services.Stealers;
using Nude.API.Services.Tickets;
using Nude.API.Services.WebHooks;
using Nude.API.Services.Workers;
using Nude.Authorization.Handlers;
using Nude.Authorization.Stores;
using Nude.Mapping.Profiles;
using Nude.Mapping.Utils;
using Nude.Parsers.Abstractions;
using Nude.Parsers.Factories;
using Nude.Parsers.HentaiChan;
using Nude.Parsers.NudeMoon;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

#region Logger

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

#endregion

#region Controllers

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => options.BindOptions());

#endregion

#region Authentication & Authorization

builder.Services.AddAuthentication(TelegramDefaults.DefaultScheme)
    .AddJwtBearer(TelegramDefaults.DefaultScheme, options =>
    {
        var config = builder.Configuration.GetRequiredSection("Authentication:Jwt");
        
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = config["Issuer"],
            ValidateAudience = true,
            ValidAudience = config["Audience"],
            ValidateLifetime = true,
            
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["KEY"])),
            ValidateIssuerSigningKey = true,
        };
    });

#endregion

#region Services

builder.Services.AddScoped<IAuthorisedMangaParserFactory<INudeParser>, NudeMoonParserFactory>();
builder.Services.AddScoped<IAuthorisedMangaParserFactory<IHentaiChanParser>, HentaiChanParserFactory>();

builder.Services.AddScoped<IAuthorizationHandler<INudeParser>, NudeMoonAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler<IHentaiChanParser>, HentaiChanAuthorizationHandler>();

builder.Services.AddScoped<ICredentialsSecureStore, CredentialsSecureStore>();

builder.Services.AddScoped<IMangaService, MangaService>();
builder.Services.AddScoped<IImagesService, ImagesService>();
builder.Services.AddScoped<IContentTicketService, ContentTicketService>();

builder.Services.AddScoped<ITagManager, TagManager>();
builder.Services.AddScoped<IMangaParserResolver, MangaParserResolver>();
builder.Services.AddScoped<IContentStealerService, ContentStealerService>();
builder.Services.AddScoped<IFormatterService, FormatterService>();

builder.Services.AddScoped<IWebHookService, WebHookService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<ITelegraphClient, DefaultTelegraphClient>();

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

app.UseAuthorization();

app.MapGet("/", async ctx =>
{
    await ctx.Response.WriteAsync("Иди нахуй");
});

app.UseMiddleware<ErrorsMiddleware>();

app.MapControllers();

app.Run();