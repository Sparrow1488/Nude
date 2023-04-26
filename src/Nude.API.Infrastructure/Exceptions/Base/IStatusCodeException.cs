namespace Nude.API.Infrastructure.Exceptions.Base;

public interface IStatusCodeException
{
    int StatusCode { get; }
}