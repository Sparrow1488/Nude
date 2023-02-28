using Microsoft.AspNetCore.Http;

namespace Nude.API.Infrastructure.Exceptions;

public class BadRequestException : ApiException, IStatusCodeException
{
    public BadRequestException()
    {
    }

    public BadRequestException(string? message) : base(message)
    {
    }

    public int StatusCode => StatusCodes.Status400BadRequest;
}