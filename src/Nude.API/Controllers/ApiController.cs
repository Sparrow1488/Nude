using Microsoft.AspNetCore.Mvc;
using Nude.API.Infrastructure.Extensions;

namespace Nude.API.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Exception(Exception exception)
    {
        var response = exception.ToResponse();
        return StatusCode(response.Status, response);
    }
}