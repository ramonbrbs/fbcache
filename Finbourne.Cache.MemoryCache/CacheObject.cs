namespace Finbourne.Cache.MemoryCache;

public class CacheObject<TKey,TContent>
{
    public TContent Content { get; set; }
    public TKey Key { get; set; }
    public CacheObject<TKey,TContent>? Next { get; set; }
    public CacheObject<TKey,TContent>? Previous { get; set; }

    public CacheObject(TKey key, TContent content)
    {
        Key = key;
        Content = content;
    }
}