namespace Nude.Authorization.Stores;

public interface ICredentialsSecureStore
{
    Task SaveAsync(string key, UserCredentials credentials);
    Task<UserCredentials?> GetAsync(string key);
}