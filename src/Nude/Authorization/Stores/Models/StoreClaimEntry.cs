namespace Nude.Authorization.Stores.Models;

public class StoreClaimEntry
{
    public StoreClaimEntry(string type, string value, string issuer)
    {
        Type = type;
        Value = value;
        Issuer = issuer;
    }
    
    public string Type { get; set; }
    public string Value { get; set; }
    public string Issuer { get; set; }
}