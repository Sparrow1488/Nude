using Microsoft.AspNetCore.Mvc;
using Nude.API.Infrastructure.Exceptions.Client;

namespace Nude.API.Controllers;

[Route("auth")]
public class AuthorizationController : ApiController
{
    [HttpPost]
    public async Task<IActionResult> SignInTelegram(string username)
    {
        throw new NotFoundException("");
    }
}