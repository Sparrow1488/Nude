using Microsoft.AspNetCore.Http;
using Nude.API.Infrastructure.Exceptions.Base;

namespace Nude.API.Infrastructure.Exceptions.Client;

public class BadRequestException : ApiException, IStatusCodeException
{
    public BadRequestException(string? message) : base(message) { }

    public BadRequestException(string? description, string? message) : base(message)
    {
        if (description is not null)
        {
            Data.Add("description", description);
        }
    }

    public int StatusCode => StatusCodes.Status400BadRequest;
}