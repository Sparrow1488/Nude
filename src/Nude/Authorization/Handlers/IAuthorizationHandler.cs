namespace Nude.Authorization.Handlers;

public interface IAuthorizationHandler<TService>
{
    // TODO: 
    // + 1. Create UserCredentials by login request
    // 2. Encrypt UserCredentials local to skip authorization in future
    // 3. Decrypt UserCredentials to use it again
    
    Task<UserCredentials> AuthorizeAsync(string login, string password);
}