using Nude.API.Infrastructure.Exceptions.Base;
using Nude.API.Models.Users;

namespace Nude.API.Services.Users;

public class UserSession : IUserSession
{
    private readonly IUsersService _service;
    private readonly HttpContext _httpContext;

    public UserSession(IUsersService service, IHttpContextAccessor accessor)
    {
        _service = service;
        _httpContext = accessor.HttpContext ?? throw new ApiException(
            $"HttpContext not available in {nameof(UserSession)}"
        );
    }
    
    public async Task<User> GetUserAsync()
    {
        var userIdClaim = _httpContext.User.FindFirst("sub")
            ?? throw new ApiException("User has no 'sub' claim");

        var userId = int.Parse(userIdClaim.Value);
        var user = await _service.GetByIdAsync(userId)
            ?? throw new ApiException("User not found in session by 'sub' claim");

        return user;
    }
}