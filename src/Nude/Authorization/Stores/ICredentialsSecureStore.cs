namespace Nude.Authorization.Stores;

public interface ICredentialsSecureStore
{
    Task<bool> ExistsAsync(string key);
    Task SaveAsync(string key, UserCredentials credentials);
    Task<UserCredentials?> GetAsync(string key);
}