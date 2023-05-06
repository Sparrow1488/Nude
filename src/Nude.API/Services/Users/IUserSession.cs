using Nude.API.Models.Users;

namespace Nude.API.Services.Users;

public interface IUserSession
{
    bool IsAuthorized();
    Task<User> GetUserAsync();
}