using Nude.Bot.Tg.Services.Users;

namespace Nude.Bot.Tg.Clients.Nude.Abstractions;

public interface IAuthorizedClientCreator
{
    IAuthorizedNudeClient AuthorizeClient(UserSession session);
}