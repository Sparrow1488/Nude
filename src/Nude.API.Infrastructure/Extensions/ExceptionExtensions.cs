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
            Data = exception.Data,
            Message = exception.Message,
            Exception = exception.GetType().Name,
            Status = StatusCodes.Status500InternalServerError
        };

        if (exception is IStatusCodeException statusCodeException)
        {
            response.Status = statusCodeException.StatusCode;
        }

        return response;
    }
}