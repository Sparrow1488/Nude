using System.Net;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nude.Models.Tickets.Parsing;
using Nude.Tg.Bot;
using Nude.Tg.Bot.Http.Routes;
using Nude.Tg.Bot.Telegram.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

var nudeBotContext = NudeBotContext.CreateDefault();
var host = nudeBotContext.GetHost();
await host.StartAsync();

var services = nudeBotContext.Services;

var bot = services.GetRequiredService<ITelegramBotClient>();
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
var config = services.GetRequiredService<IConfiguration>();
listener.Prefixes.Add(config["Http:BaseUrl"] + "/");

while (true)
{
    try
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
    catch(Exception ex)
    {
        logger.LogError(ex, "In Http Listener error");
    }
}