namespace Nude.Authorization.Handlers;

public interface IAuthorizationHandler<TService>
{
    Task<UserCredentials> AuthorizeAsync(string login, string password);
}