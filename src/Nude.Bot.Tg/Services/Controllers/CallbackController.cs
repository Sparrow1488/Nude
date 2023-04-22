using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Nude.API.Infrastructure.Configurations.Json;
using Nude.API.Models.Notifications;
using Nude.Bot.Tg.Services.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nude.Bot.Tg.Services.Controllers
{
    public class CallbackController
    {
        private readonly IServiceProvider _provider;
        private readonly HttpContext _httpContext;

        public CallbackController(IServiceProvider provider, HttpContext httpContext)
        {
            _provider = provider;
            _httpContext = httpContext;
        }

        public async Task ProcessCallback()
        {
            var obj = await GetDeserializedObject();
            var callbackRoute = _provider.GetRequiredService<ICallbackHandler>();
            await callbackRoute.OnCallbackAsync(obj);

            _httpContext.Response.StatusCode = StatusCodes.Status200OK;
            await _httpContext.Response.WriteAsync("ok");
        }

        private async Task<Notification> GetDeserializedObject()
        {
            using var content = new StreamContent(_httpContext.Request.Body);
            var subjectJson = await content.ReadAsStringAsync();

            var subject = JsonConvert.DeserializeObject<Notification>(
                subjectJson,
                JsonSettingsProvider.CreateDefault()
            );

            return subject!;
        }
    }
}
