using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Services.Seeds;
using Nude.API.Models.Users;
using Nude.API.Services.Users;

namespace Nude.API.Services.Seeds;

public class DataSeeder : IDataSeeder
{
    private readonly IUserService _user;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(
        IUserService user,
        ILogger<DataSeeder> logger)
    {
        _user = user;
        _logger = logger;
    }
    
    public Task SeedDataAsync()
    {
        return SeedAdminsAsync();
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
}