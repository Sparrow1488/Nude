using Microsoft.AspNetCore.Http;
using Nude.API.Infrastructure.Exceptions.Base;

namespace Nude.API.Infrastructure.Exceptions.Client;

public class BadRequestException : ApiException, IStatusCodeException
{
    public BadRequestException(string? message) : base(message) { }

    public int StatusCode => StatusCodes.Status400BadRequest;
}