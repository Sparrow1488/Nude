using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Users;

namespace Nude.API.Services.Users.Results;

public class UserCreationResult : ServiceResult<User>
{
    public UserCreationResult(Exception exception) : base(exception)
    {
    }

    public UserCreationResult(User result) : base(result)
    {
    }
}