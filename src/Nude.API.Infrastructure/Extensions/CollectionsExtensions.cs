namespace Nude.API.Infrastructure.Extensions;

public static class CollectionsExtensions
{
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        if (collection.IsReadOnly) return;
        
        foreach (var item in items)
            collection.Add(item);
    }
}