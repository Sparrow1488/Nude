namespace Nude.Authorization.Stores.Models;

public class StoreUserCredentials
{
    public ICollection<StoreClaim>? Claims { get; set; }
    public string Domain { get; set; }
    public Schema Schema { get; set; }
}