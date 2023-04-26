using Nude.API.Infrastructure.Exceptions.Base;

namespace Nude.API.Infrastructure.Exceptions.Internal;

public class BadServerResponseException : ApiException
{
    public BadServerResponseException(string? message) : base(message) { }
}