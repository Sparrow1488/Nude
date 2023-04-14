using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Nude.API.Contracts.Tokens.Responses;
using Nude.API.Infrastructure.Services.Keys;
using Nude.API.Models.Users.Accounts;
using Nude.API.Services.Users;

namespace Nude.API.Controllers;

[Route("auth")]
public class AuthorizationController : ApiController
{
    private readonly IUsersService _usersService;

    public AuthorizationController(IUsersService usersService)
    {
        _usersService = usersService;
    }
    
    [HttpPost]
    public async Task<IActionResult> SignInTelegram(string username)
    {
        var handler = new JsonWebTokenHandler();

        var existsUser = await _usersService.FindByTelegramAsync(username);

        if (existsUser == null)
        {
            var account = new TelegramAccount { Username = username };
            var result = await _usersService.CreateAsync(account);
            
            if (!result.IsSuccess)
            {
                return Exception(result.Exception!);
            }
            
            existsUser = result.Result!;
        }

        var credentials = new SigningCredentials(CreateSecurityKey(), SecurityAlgorithms.RsaSha256);
        var telegramAccount = (TelegramAccount) existsUser.Accounts.First(x => x is TelegramAccount);
        var token = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "http://127.0.0.1:3001",
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(new []
            {
                new Claim("sub", existsUser.Id.ToString()),
                new Claim("name", telegramAccount.Username),
            })
        });

        var response = new JwtTokenResponse { Token = token };
        return Ok(response);
    }

    private static SecurityKey CreateSecurityKey()
    {
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(KeysProvider.GetPrivateKey(), out _);
        return new RsaSecurityKey(rsa);
    }

    [HttpGet("username"), Authorize]
    public string? GetUsername()
    {
        return HttpContext.User.FindFirst("name")?.Value;
    }
}