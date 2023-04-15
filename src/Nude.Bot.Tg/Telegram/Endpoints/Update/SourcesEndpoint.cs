using Nude.API.Infrastructure.Utility;
using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class SourcesEndpoint : TelegramUpdateCommandEndpoint
{
    public SourcesEndpoint() : base("/sources") { }

    public override async Task HandleAsync() =>
        await MessageAsync(await MessagesStore.GetSourcesMessageAsync(ContentAware.Domains.ToList()));
}