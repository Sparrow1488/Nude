using System.Reflection;
using System.Text.Json;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nude.API.Background;
using Nude.API.Data.Contexts;
using Nude.API.Data.Managers;
using Nude.API.Data.Repositories;
using Nude.API.Infrastructure.Middlewares;
using Nude.API.Infrastructure.Services.FeedBack;
using Nude.API.Services.Manga;
using Nude.API.Services.Parsing;
using Nude.Mapping.Profiles;
using Nude.Mapping.Utils;
using Nude.Parsers;
using Quartz;
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

const string section = "Credentials:NudeMoon";
var fusionUser = builder.Configuration.GetValue<string>($"{section}:FusionUser")!;
var sessionId = builder.Configuration.GetValue<string>($"{section}:PhpSessionId")!;
var parser = await NudeParser.CreateAsync(fusionUser, sessionId);

builder.Services.AddSingleton<INudeParser>(_ => parser);

builder.Services.AddScoped<IMangaService, NudeMoonService>();
builder.Services.AddScoped<IParsingTicketsService, ParsingTicketsService>();

builder.Services.AddScoped<IFeedBackService, CallbackService>();

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

builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<ITagManager, TagManager>();

#region Mappers

var profilesAssembly = typeof(MangaProfile).Assembly;
MapperInitializer.AssertConfigurationIsValid(profilesAssembly);
builder.Services.AddAutoMapper(x => x.AddMaps(profilesAssembly));

#endregion

#region Quartz

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();
    
    var jobKey = new JobKey("Nude-Moon Parser");
    q.AddJob<ParsingBgService>(opts => opts.WithIdentity(jobKey));
    
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("Nude-Moon Parser Identity")
        .WithSchedule(SimpleScheduleBuilder.Create())
    );
});
builder.Services.AddQuartzHostedService(q =>
{
    q.WaitForJobsToComplete = false;
    // q.StartDelay = TimeSpan.FromSeconds(5);
});

#endregion

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.UseMiddleware<ErrorsMiddleware>();

app.MapControllers();

app.Run();