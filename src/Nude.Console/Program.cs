using System.Reflection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Nude.Models;
using Nude.Parsers;
using Serilog;

#region Configuration

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

Log.Information("NudeApp started!");

const string section = "Credentials:NudeMoon";
var fusionUser = configuration.GetValue<string>($"{section}:FusionUser")!;
var sessionId = configuration.GetValue<string>($"{section}:PhpSessionId")!;
using INudeParser parser = await NudeParser.CreateAsync(fusionUser, sessionId);

#endregion

var mangaUrls = new List<string>
{
    "https://nude-moon.org/20932-online--gingko-chibi-succu-shiko-life-nioi-de-ecchi-na-kibun-ni-sase.html?row"
};

var results = new List<Manga>();
foreach (var url in mangaUrls)
{
    Log.Information("Parsing {url}", url);
    
    var manga = await parser.GetByUrlAsync(url);
    results.Add(manga);
    
    Log.Information("Success");
}

#region Output

var jsonResult = JsonConvert.SerializeObject(results, new JsonSerializerSettings
{
    Formatting = Formatting.Indented
});
Console.WriteLine(jsonResult);

#endregion