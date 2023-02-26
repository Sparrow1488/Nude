using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Nude.Providers;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Information("NudeApp started!");

const string section = "Credentials:NudeMoon";
var fusionUser = configuration.GetValue<string>($"{section}:FusionUser")!;
var sessionId = configuration.GetValue<string>($"{section}:PhpSessionId")!;
using INudeParser parser = await NudeParser.CreateAsync(fusionUser, sessionId);

// var result = await parser.GetAsync(0, 5);
var result = await parser.GetByUrlAsync("https://nude-moon.org/3833--.html");
var jsonResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings
{
    Formatting = Formatting.Indented
});
Console.WriteLine(jsonResult);