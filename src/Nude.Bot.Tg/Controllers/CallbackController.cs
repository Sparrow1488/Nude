using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nude.API.Infrastructure.Configurations.Json;
using Nude.API.Models.Notifications;
using Nude.Bot.Tg.Services.Handlers;

namespace Nude.Bot.Tg.Controllers;

[ApiController, Route("/callback")]
public class CallbackController : ControllerBase
{
    private readonly ICallbackHandler _callbackHandler;

    public CallbackController(ICallbackHandler callbackHandler) =>
        _callbackHandler = callbackHandler;

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var notification = await GetDeserializedObject();
        await _callbackHandler.HandleAsync(notification);

        return Ok("ok");
    }

    private async Task<Notification> GetDeserializedObject()
    {
        using var content = new StreamContent(HttpContext.Request.Body);
        var subjectJson = await content.ReadAsStringAsync();

        var subject = JsonConvert.DeserializeObject<Notification>(
            subjectJson,
            JsonSettingsProvider.CreateDefault()
        );

        return subject!;
    }
}