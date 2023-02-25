using Nude.API.Services.Manga;
using Nude.Providers;

var builder = WebApplication.CreateBuilder(args);

const string section = "Credentials:NudeMoon";
var fusionUser = builder.Configuration.GetValue<string>($"{section}:FusionUser")!;
var sessionId = builder.Configuration.GetValue<string>($"{section}:PhpSessionId")!;
var parser = await NudeParser.CreateAsync(fusionUser, sessionId);

builder.Services.AddScoped<IMangaService, NudeMoonService>();
builder.Services.AddSingleton<INudeParser>(_ => parser);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();