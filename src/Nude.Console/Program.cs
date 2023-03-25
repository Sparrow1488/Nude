using System.Reflection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Nude.Authorization.Handlers;
using Nude.Authorization.Stores;
using Nude.Models;
using Nude.Parsers.Abstractions;
using Nude.Parsers.Factories;
using Nude.Parsers.HentaiChan;
using Nude.Parsers.NudeMoon;
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

#endregion

using IMangaParser parser = await CreateHentaiChanParser();

var mangaUrls = new List<string>
{
    // "https://nude-moon.org/20932-online--gingko-chibi-succu-shiko-life-nioi-de-ecchi-na-kibun-ni-sase.html?row",
    "https://y.hentaichan.live/online/45195-zimnie-kanikuly.html?cacheId=1679404899",
    // "https://y.hentaichan.live/manga/45195-zimnie-kanikuly.html",
    // "https://y.hentaichan.live/manga/45217-mikasa.html",
    // "https://y.hentaichan.live/manga/45190-kniga-.html"
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

#region Parsers

async Task<INudeParser> CreateNudeParser()
{
    const string section = "Credentials:NudeMoon";
    var login = configuration.GetValue<string>($"{section}:Login")!;
    var password = configuration.GetValue<string>($"{section}:Password")!;
    
    var secureStore = new CredentialsSecureStore();
    var authHandler = new NudeMoonAuthorizationHandler();
    var factory = new NudeMoonParserFactory(secureStore, authHandler);
    
    return await factory.CreateAuthorizedAsync(login, password);
}

async Task<IHentaiChanParser> CreateHentaiChanParser()
{
    const string section = "Credentials:HentaiChan";
    var login = configuration.GetValue<string>($"{section}:Login")!;
    var password = configuration.GetValue<string>($"{section}:Password")!;

    var secureStore = new CredentialsSecureStore();
    var authHandler = new HentaiChanAuthorizationHandler();
    var factory = new HentaiChanParserFactory(secureStore, authHandler);
    
    return await factory.CreateAuthorizedAsync(login, password);
}

#endregion