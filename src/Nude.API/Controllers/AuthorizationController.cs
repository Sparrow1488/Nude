using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Nude.API.Contracts.Tokens.Responses;
using Nude.API.Infrastructure.Services.Keys;

namespace Nude.API.Controllers;

[Route("auth")]
public class AuthorizationController : ApiController
{
    [HttpPost]
    public IActionResult SignInTelegram(string username)
    {
        var handler = new JsonWebTokenHandler();

        var credentials = new SigningCredentials(CreateSecurityKey(), SecurityAlgorithms.RsaSha256);
        var token = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "http://127.0.0.1:3001",
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(new []
            {
                new Claim("sub", Guid.NewGuid().ToString()),
                new Claim("name", username),
            })
        });

        var response = new JwtTokenResponse { Token = token };
        return Ok(response);
    }

    private SecurityKey CreateSecurityKey()
    {
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(KeysProvider.GetPrivateKey(), out _);
        return new RsaSecurityKey(rsa);
    }

    [HttpGet("username")]
    public string? GetUsername()
    {
        return HttpContext.User.FindFirst("name")?.Value;
    }
}