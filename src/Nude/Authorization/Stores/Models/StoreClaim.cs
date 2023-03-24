namespace Nude.Authorization.Stores.Models;

public class StoreClaim
{
    public StoreClaim(string type, string value)
    {
        Type = type;
        Value = value;
    }
    
    public string Type { get; set; }
    public string Value { get; set; }
}