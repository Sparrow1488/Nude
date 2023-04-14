using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Models.Users;
using Nude.API.Models.Users.Accounts;
using Nude.API.Services.Users.Results;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.API.Services.Users;

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