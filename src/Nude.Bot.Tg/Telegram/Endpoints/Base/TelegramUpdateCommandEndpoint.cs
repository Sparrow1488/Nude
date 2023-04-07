namespace Nude.Bot.Tg.Telegram.Endpoints.Base;

public abstract class TelegramUpdateCommandEndpoint : TelegramUpdateEndpoint
{
    private readonly string _command;

    public TelegramUpdateCommandEndpoint(string command)
    {
        _command = command;
    }

    public override bool CanHandle() => MessageText?.StartsWith(_command) ?? false;
}