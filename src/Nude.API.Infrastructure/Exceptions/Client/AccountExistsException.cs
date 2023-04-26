namespace Nude.API.Infrastructure.Exceptions.Client;

public class AccountExistsException : BadRequestException
{
    public AccountExistsException(string? message) : base(message)
    {
    }
}