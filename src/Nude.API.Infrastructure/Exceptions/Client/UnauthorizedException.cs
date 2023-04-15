using Microsoft.AspNetCore.Http;
using Nude.API.Infrastructure.Exceptions.Base;

namespace Nude.API.Infrastructure.Exceptions.Client;

public class UnauthorizedException : ApiException, IStatusCodeException
{
    public UnauthorizedException()
    {
    }

    public UnauthorizedException(string? message) : base(message)
    {
    }

    public int StatusCode => StatusCodes.Status401Unauthorized;
}