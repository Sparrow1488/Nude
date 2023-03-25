namespace Nude.Authorization.Stores.Models;

public class StoreUserCredentials
{
    public ICollection<StoreClaimEntry>? Claims { get; set; }
    public Schema Schema { get; set; }
}