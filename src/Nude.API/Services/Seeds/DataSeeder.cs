using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Services.Seeds;
using Nude.API.Models.Users;
using Nude.API.Services.Blacklists;
using Nude.API.Services.Users;
using Nude.Data.Infrastructure.Contexts;

namespace Nude.API.Services.Seeds;

public class DataSeeder : IDataSeeder
{
    private readonly AppDbContext _context;
    private readonly IBlacklistService _blacklistService;
    private readonly IUserService _user;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(
        AppDbContext context,
        IBlacklistService blacklistService,
        IUserService user,
        ILogger<DataSeeder> logger)
    {
        _context = context;
        _blacklistService = blacklistService;
        _user = user;
        _logger = logger;
    }
    
    public async Task SeedDataAsync()
    {
        await SeedAdminsAsync();
        await SetBlacklistsAsync();
    }
    
    private async Task SeedAdminsAsync()
    {
        _logger.LogInformation("Seeding default admin users");
        
        var sparrowUser = await _user.FindByTelegramAsync("sprw1488");
        var webDevUser = await _user.FindByTelegramAsync("WebDevO");

        var admins = new List<User>();
        if (sparrowUser != null)
            admins.Add(sparrowUser);
        if (webDevUser != null)
            admins.Add(webDevUser);
        
        _logger.LogInformation(
            "Found {count} users in DB. Setting {claim} claims them",
            admins.Count,
            NudeClaims.Role.Administrator
        );

        foreach (var admin in admins)
        {
            await _user.SetClaimAsync(
                admin, 
                NudeClaimTypes.Role, 
                NudeClaims.Role.Administrator
            );
        }
    }

    /// <summary>
    /// Нужно для добавления черного списка старым пользователям. Может быть удалено в будущем.
    /// </summary>
    private async Task SetBlacklistsAsync()
    {
        var usersWithoutBlacklists = await _context.Users
            .Where(x => x.Blacklist == null)
            .ToArrayAsync();
        
        _logger.LogInformation("Found {count} users without blacklists", usersWithoutBlacklists.Length);

        foreach (var user in usersWithoutBlacklists)
        {
            var defaultBlacklist = await _blacklistService.GetDefaultAsync();
            await _blacklistService.CreateAsync(user, defaultBlacklist.Tags);
        }
    }
}