using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Nude.API.Contracts.Tokens.Responses;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Services.Keys;
using Nude.API.Models.Users;
using Nude.API.Models.Users.Accounts;
using Nude.API.Services.Users;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.API.Controllers;

[Route("auth")]
public class AuthorizationController : ApiController
{
    private readonly IUserService _userService;

    public AuthorizationController(IUserService userService) =>
        _userService = userService;
    
    [HttpPost]
    public async Task<IActionResult> SignInTelegram(string username)
    {
        var handler = new JsonWebTokenHandler();

        var existsUser = await _userService.FindByTelegramAsync(username);

        if (existsUser is null)
        {
            var account = new TelegramAccount { Username = username };
            var result = await _userService.CreateAsync(account);
            
            if (!result.IsSuccess)
            {
                return Exception(result.Exception!);
            }
            
            existsUser = result.Result!;

            await _userService.SetClaimAsync(
                existsUser, 
                NudeClaimTypes.Role,
                NudeClaims.Role.User,
                issuer: null
            );
        }
        
        var credentials = new SigningCredentials(CreateSecurityKey(), SecurityAlgorithms.RsaSha256);
        var telegramAccount = existsUser.Accounts.OfType<TelegramAccount>().First();
        var token = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "http://127.0.0.1:3001",
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(GetClaims(existsUser, telegramAccount))
        });

        var response = new JwtTokenResponse { Token = token };
        return Ok(response);
    }

    private static ICollection<Claim> GetClaims(User user, TelegramAccount telegramAccount)
    {
        var claims = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("name", telegramAccount.Username)
        };
        claims.AddRange(user.Claims.ToClaims());

        return claims;
    }

    private static SecurityKey CreateSecurityKey()
    {
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(SecurityKeysProvider.GetPrivateKey(), out _);
        return new RsaSecurityKey(rsa);
    }

    [HttpGet("username"), Authorize]
    public string? GetUsername()
    {
        return HttpContext.User.FindFirst("name")?.Value;
    }
}