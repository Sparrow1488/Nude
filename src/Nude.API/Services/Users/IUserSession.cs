using Nude.API.Models.Users;

namespace Nude.API.Services.Users;

public interface IUserSession
{
    Task<User> GetUserAsync();
}