using Nude.API.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nude.Bot.Tg.Services.Handlers
{
    public interface ICallbackHandler
    {
        public Task OnCallbackAsync(Notification subject);
    }
}
