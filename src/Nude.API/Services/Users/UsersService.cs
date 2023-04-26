using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Models.Claims;
using Nude.API.Models.Users;
using Nude.API.Models.Users.Accounts;
using Nude.API.Services.Users.Results;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.API.Services.Users;

#region Rider annotations

// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

#endregion

public class UsersService : IUsersService
{
    private readonly AppDbContext _context;

    public UsersService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserCreationResult> CreateAsync(Account account)
    {
        if (account is TelegramAccount telegramAccount)
        {
            var username = telegramAccount.Username;
            
            var exists = await FindByTelegramAsync(username);
            if (exists != null)
            {
                var exception = new AccountExistsException(
                    $"Telegram account with name '{username}' already exists"
                );
                return new UserCreationResult(exception);
            }

            var user = new User
            {
                Accounts = new List<Account> { account }
            };
            
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserCreationResult(user);
        }

        throw new NotImplementedException();
    }

    public async Task<ClaimEntry> SetClaimAsync(User user, string type, string value, string? issuer = null)
    {
        if (user.Claims is null)
        {
            await _context.Entry(user).Collection(nameof(user.Claims)).LoadAsync();
        }

        var claim = new ClaimEntry
        {
            Type = type,
            Value = value,
            Issuer = issuer,
            User = user
        };
        
        var userClaim = user.Claims!.FirstOrDefault(x => x.Type == type);
        if (userClaim is not null)
        {
            await DeleteClaimAsync(userClaim);
        }

        await _context.AddAsync(claim);
        await _context.SaveChangesAsync();

        return claim;
    }

    public async Task DeleteClaimAsync(ClaimEntry claim)
    {
        _context.Remove(claim);
        await _context.SaveChangesAsync();
    }

    public Task<User?> GetByIdAsync(int id)
    {
        return _context.Users
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> FindByTelegramAsync(string username)
    {
        var telegramAccount = await _context.TelegramAccounts
            .Include(x => x.Owner)
            .FirstOrDefaultAsync(x => x.Username == username);

        if (telegramAccount != null)
        {
            return await GetByIdAsync(telegramAccount.Owner.Id);
        }

        return null;
    }
}