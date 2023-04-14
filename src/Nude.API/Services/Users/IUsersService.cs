using Nude.API.Models.Users;
using Nude.API.Models.Users.Accounts;
using Nude.API.Services.Users.Results;

namespace Nude.API.Services.Users;

public interface IUsersService
{
    Task<UserCreationResult> CreateAsync(Account account);
    Task<User?> GetByIdAsync(int id);
    Task<User?> FindByTelegramAsync(string username);
}