namespace Nude.Authorization.Stores.Models;

public class StoreClaim
{
    public StoreClaim(string type, string value, string domain)
    {
        Type = type;
        Value = value;
        Domain = domain;
    }
    
    public string Type { get; set; }
    public string Domain { get; set; }
    public string Value { get; set; }
}