using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Infrastructure.Constants;

namespace Nude.API.Controllers;

[ApiController, Route("auth")]
public class AuthController : ControllerBase
{
    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        var result = Challenge(TelegramDefaults.DefaultScheme);
        return Ok();
    }

    [HttpGet("test"), Authorize]
    public IActionResult Test()
    {
        return Ok("OK");
    }
}