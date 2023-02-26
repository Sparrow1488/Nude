using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Nude.API.Contracts.Errors.Responses;
using Nude.API.Infrastructure.Exceptions;

namespace Nude.API.Infrastructure.Middlewares;

public class ErrorsMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch(Exception ex)
        {
            if (ex is ApiException apiEx)
            {
                var error = new ErrorResponse
                {
                    Message = apiEx.Message,
                    Data = apiEx.Data,
                    Status = 500
                };
                if (apiEx is IStatusCodeException statusEx)
                {
                    error.Status = statusEx.StatusCode;
                }

                context.Response.StatusCode = error.Status;
                var json = JsonConvert.SerializeObject(error, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(json);
            }
            else
            {
                throw;
            }
        }
    }
}