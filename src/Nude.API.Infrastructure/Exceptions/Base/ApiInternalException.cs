namespace Nude.API.Infrastructure.Exceptions.Base;

public class ApiInternalException : ApiException
{
    public ApiInternalException() { }
    public ApiInternalException(string? message) : base(message) { }
}