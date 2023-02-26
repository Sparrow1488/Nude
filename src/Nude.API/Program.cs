using System.Reflection;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Nude.API.Data.Contexts;
using Nude.API.Data.Repositories;
using Nude.API.Infrastructure.Middlewares;
using Nude.API.Jobs;
using Nude.API.Services.Manga;
using Nude.API.Services.Parsing;
using Nude.Mapping.Profiles;
using Nude.Mapping.Utils;
using Nude.Providers;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

const string section = "Credentials:NudeMoon";
var fusionUser = builder.Configuration.GetValue<string>($"{section}:FusionUser")!;
var sessionId = builder.Configuration.GetValue<string>($"{section}:PhpSessionId")!;
var parser = await NudeParser.CreateAsync(fusionUser, sessionId);

builder.Services.AddScoped<IMangaService, NudeMoonService>();
builder.Services.AddScoped<IMangaParsingService, MangaParsingService>();
builder.Services.AddSingleton<INudeParser>(_ => parser);

builder.Services.AddDbContext<AppDbContext>(
    x => x.UseNpgsql("Server=127.0.0.1;Port=5432;Database=Nude.API.Database;Uid=postgres;Pwd=secret;", b => b.MigrationsAssembly("Nude.API")));

builder.Services.AddScoped<IMangaRepository, MangaRepository>();

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
    q.AddJob<ParsingJob>(opts => opts.WithIdentity(jobKey));
    
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("Nude-Moon Parser Identity")
        .WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(20))
    );
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

#endregion

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.UseMiddleware<ErrorsMiddleware>();

app.MapControllers();

app.Run();