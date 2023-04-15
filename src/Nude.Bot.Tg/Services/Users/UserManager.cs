using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Users;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Services.Users.Results;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.Bot.Tg.Services.Users;

public class UserManager : IUserManager
{
    private readonly BotDbContext _context;
    private readonly INudeClient _nudeClient;

    public UserManager(
        BotDbContext context,
        INudeClient nudeClient)
    {
        _context = context;
        _nudeClient = nudeClient;
    }
    
    public async Task<UserSessionResult> GetUserSessionAsync(long userId, string username)
    {
        var user = await FindByUserIdAsync(userId);
        if (user == null)
        {
            var tokenResult = await _nudeClient.AuthorizeAsync(username);
            var result = await CreateAsync(userId, username, tokenResult.ResultValue.Token);
            user = result.Result!;
        }

        return new UserSessionResult(new UserSession(user));
    }

    public async Task<UserCreationResult> CreateAsync(long userId, string username, string accessToken)
    {
        var exists = await _context.Users.AnyAsync(x => x.UserId == userId);
        if (exists)
        {
            var exception = new InvalidOperationException($"User '{userId}' already exists");
            return new UserCreationResult(exception);
        }
        
        var user = new TelegramUser
        {
            UserId = userId,
            Username = username,
            AccessToken = accessToken
        };

        await _context.AddAsync(user);
        await _context.SaveChangesAsync();

        return new UserCreationResult(user);
    }

    public Task<TelegramUser?> FindByUserIdAsync(long userId)
    {
        return _context.Users
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}