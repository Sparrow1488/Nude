namespace Nude.API.Infrastructure.Exceptions;

public interface IStatusCodeException
{
    int StatusCode { get; }
}