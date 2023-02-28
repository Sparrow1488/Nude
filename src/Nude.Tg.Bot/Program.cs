using System.Net;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nude.Models.Tickets;
using Nude.Tg.Bot;
using Nude.Tg.Bot.Handlers;
using Nude.Tg.Bot.Routes;
using Telegram.Bot;
using Telegram.Bot.Polling;

var nudeBotContext = NudeBotContext.CreateDefault();
var services = nudeBotContext.Services;

var bot = services.GetRequiredService<TelegramBotClient>();
var botInfo = await bot.GetMeAsync();

var logger = services.GetRequiredService<ILogger<Program>>();
logger.LogInformation(botInfo.FirstName + " started");

var handler = services.GetRequiredService<ITelegramHandler>();

bot.StartReceiving(
    handler.HandleUpdateAsync, 
    handler.HandleErrorAsync, 
    new ReceiverOptions()
);

using var listener = new HttpListener();
listener.Start();
listener.Prefixes.Add("http://127.0.0.1:7001/");
// var callback = new CallbackRoute();

while (true)
{
    var context = await listener.GetContextAsync();
    var request = context.Request;
    
    if (request.Url?.AbsolutePath.StartsWith("/callback") ?? false)
    {
        logger.LogInformation("Get request to '{url}'", request.Url.PathAndQuery);
        
        var queryString = new Uri(request.Url.ToString()).Query;
        var query = HttpUtility.ParseQueryString(queryString);
        var ticketId = query["ticket_id"];
        var status = query["status"];

        if (string.IsNullOrWhiteSpace(ticketId) || string.IsNullOrWhiteSpace(status))
        {
            logger.LogWarning("Received empty ticket_id or status from server");
        }
        else
        {
            var cb = services.GetRequiredService<CallbackRoute>();
            var enumStatus = Enum.Parse<ParsingStatus>(status);
            await cb.OnCallbackAsync(int.Parse(ticketId), enumStatus);
        }
        
        context.Response.StatusCode = StatusCodes.Status202Accepted;
        context.Response.ContentType = "application/text";

        var responseData = Encoding.UTF8.GetBytes("OK");
        await context.Response.OutputStream.WriteAsync(responseData);
    }
    
    context.Response.Close();
}