using System.Net;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nude.Models.Tickets.Parsing;
using Nude.Tg.Bot.Http.Routes;

namespace Nude.Tg.Bot.Http;

public class HttpServer
{
    public static async Task StartListenAsync(IServiceProvider services)
    {
        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<HttpServer>>();
        
        using var listener = new HttpListener();
        listener.Prefixes.Add(configuration["Http:BaseUrl"] + "/");

        listener.Start();

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
                logger.LogError(ex, "Oops.. Something went wrong");
            }
        }
    }
}