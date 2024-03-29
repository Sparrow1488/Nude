﻿using System.Reflection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Nude.Constants;
using Nude.Creators;
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

#endregion

var (login, password) = GetCredentials(HentaiChanDefaults.Name);

var creator = new ParserCreator();
using IMangaParser parser = await creator.CreateHentaiChanAsync(login, password, true);

var mangaUrls = new List<string>
{
    // "https://nude-moon.org/20932-online--gingko-chibi-suc.html",
    // "https://nude-moon.org/21446--pigmanboy-rika-no-oshiri-challenge--ispetanie-dla-riki.html",
    "https://xxxxx.hentaichan.live/manga/45442-msultimate.html",
    "https://y.hentaichan.live/online/45195-zimnie-kanikuly.html?cacheId=1679404899",
    "https://xxxxx.hentaichan.live/manga/45420-vlyubitsya-v-mori-summer.html",
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

(string login, string password) GetCredentials(string siteName)
{
    var section = $"Credentials:{siteName}";
    var login = configuration.GetValue<string>($"{section}:Login")!;
    var password = configuration.GetValue<string>($"{section}:Password")!;
    
    return (login, password);
}

#endregion