using Microsoft.AspNetCore.Http;
using Nude.API.Contracts.Errors.Responses;
using Nude.API.Infrastructure.Exceptions.Base;

namespace Nude.API.Infrastructure.Extensions;

public static class ExceptionExtensions
{
    public static ErrorResponse ToResponse(this Exception exception)
    {
        var response = new ErrorResponse
        {
            Message = exception.Message,
            Data = exception.Data,
            Status = StatusCodes.Status500InternalServerError
        };

        if (exception is IStatusCodeException statusCodeException)
        {
            response.Status = statusCodeException.StatusCode;
        }

        return response;
    }
}