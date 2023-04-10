namespace Nude.API.Infrastructure.Utility;

public class ContentKeyHelper
{
    public static string CreateContentKey(string entityType, string row)
    {
        return entityType + "-" + row.ToUpper();
    }
}