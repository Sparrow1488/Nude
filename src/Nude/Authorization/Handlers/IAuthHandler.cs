namespace Nude.Authorization.Handlers;

public interface IAuthHandler<TService>
{
    Task<UserCredentials> AuthorizeAsync(string login, string password);
}