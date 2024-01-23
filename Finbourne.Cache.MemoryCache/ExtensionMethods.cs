namespace Finbourne.Cache.MemoryCache;

public static class ExtensionMethods
{
    /// <summary>
    /// Try to get item from cache, if not found get from queryable and add to cache
    /// </summary>
    /// <param name="cache">Cache object</param>
    /// <param name="key">Search key used for cache</param>
    /// <param name="queryable">Queryable object to be used if not found in cache</param>
    /// <param name="predicate">Predicate to be applied for first or default on the queryable object</param>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TContent">Content type</typeparam>
    /// <returns></returns>
    public static TContent? GetFromCacheOrGetFirstOrDefault<TKey,TContent>(this MemoryCache<TKey,TContent> cache, TKey key, 
        IEnumerable<TContent> queryable, Func<TContent, bool> predicate)
    {
        
        var cachedItem = cache.Get(key);
        if (cachedItem != null) return cachedItem.Content;
        
        var item = queryable.FirstOrDefault(predicate);
        if (item is not null)
        {
            cache.Add(new CacheObject<TKey, TContent>(key, item));
        }

        return item;
    }
}