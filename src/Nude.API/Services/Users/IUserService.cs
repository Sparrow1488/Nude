using Nude.API.Models.Claims;
using Nude.API.Models.Users;
using Nude.API.Models.Users.Accounts;
using Nude.API.Services.Users.Results;

namespace Nude.API.Services.Users;

public interface IUserService
{
    Task<UserCreationResult> CreateAsync(Account account);
    Task<ClaimEntry> SetClaimAsync(User user, string type, string value, string? issuer = null);
    Task DeleteClaimAsync(ClaimEntry claim);
    Task<User?> GetByIdAsync(int id);
    Task<User?> FindByTelegramAsync(string username);
}