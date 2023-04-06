namespace Nude.API.Services.Authentication;

public interface IAuthenticationService
{
    Task<string> AuthenticateAsync();
}