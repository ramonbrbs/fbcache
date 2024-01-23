namespace Finbourne.Cache.MemoryCache;

public static class ExtensionMethods
{
    public static TContent? GetFromCacheOrGetFirstOrDefault<TKey,TContent>(this MemoryCache<TKey,TContent> cache, TKey k, 
        IEnumerable<TContent> queryable, Func<TContent, bool> predicate)
    {
        
        var cachedItem = cache.Get(k);
        if (cachedItem != null) return cachedItem.Content;
        
        var item = queryable.FirstOrDefault(predicate);
        if (item is not null)
        {
            cache.Add(new CacheObject<TKey, TContent>(k, item));
        }

        return item;
    }
}