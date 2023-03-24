﻿using System.Drawing.Text;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Nude.Authorization.Handlers;
using Nude.Models;
using Nude.Parsers;
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

var authHandler = new HentaiChanAuthorizationHandler();
var credentials = await authHandler.AuthorizeAsync();

using var parser = await CreateNudeParser();

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

#region Parsers

async Task<INudeParser> CreateNudeParser()
{
    const string section = "Credentials:NudeMoon";
    var fusionUser = configuration.GetValue<string>($"{section}:FusionUser")!;
    var sessionId = configuration.GetValue<string>($"{section}:PhpSessionId")!;
    return await NudeParser.CreateAsync(fusionUser, sessionId);
}

async Task<IHentaiChanParser> CreateHentaiChanParser()
{
    const string section = "Credentials:HentaiChan";
    var fusionUser = configuration.GetValue<string>($"{section}:Dle")!;
    var sessionId = configuration.GetValue<string>($"{section}:PhpSessionId")!;
    return await HentaiChanParser.CreateAsync(null, null, null, null);
}

#endregion