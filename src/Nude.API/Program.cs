using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nude.API.Data.Contexts;
using Nude.API.Data.Managers;
using Nude.API.Data.Repositories;
using Nude.API.Infrastructure.Extensions;
using Nude.API.Infrastructure.Middlewares;
using Nude.API.Infrastructure.Services.FeedBack;
using Nude.API.Services.Manga;
using Nude.API.Services.Parsing;
using Nude.API.Services.Resolvers;
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

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Formatting = Formatting.Indented;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };
    });

#endregion

#region Services

builder.Services.AddScoped<IAuthorisedMangaParserFactory<INudeParser>, NudeMoonParserFactory>();
builder.Services.AddScoped<IAuthorisedMangaParserFactory<IHentaiChanParser>, HentaiChanParserFactory>();

builder.Services.AddScoped<IAuthorizationHandler<INudeParser>, NudeMoonAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler<IHentaiChanParser>, HentaiChanAuthorizationHandler>();

builder.Services.AddScoped<IMangaParserResolver, MangaParserResolver>();

builder.Services.AddScoped<ICredentialsSecureStore, CredentialsSecureStore>();

builder.Services.AddScoped<IMangaService, MangaService>();
builder.Services.AddScoped<IParsingTicketsService, ParsingTicketsService>();

builder.Services.AddScoped<IFeedBackService, CallbackService>();

builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<ITagManager, TagManager>();

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

builder.Services.AddBackgroundWorker<ParsingBackgroundWorker>();

#endregion

var app = builder.Build();

app.UseAuthorization();

app.UseMiddleware<ErrorsMiddleware>();

app.MapControllers();

app.Run();