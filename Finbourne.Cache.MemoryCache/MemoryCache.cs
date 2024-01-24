namespace Finbourne.Cache.MemoryCache;

public class MemoryCache<TKey,TContent>
{
    private readonly long _sizeLimit;
    private readonly bool _debugItems;

    public Dictionary<TKey, CacheObject<TKey, TContent>?> Items { get; set; } = new Dictionary<TKey, CacheObject<TKey, TContent>?>();
    public CacheObject<TKey, TContent>? Head { get; set; } = null;
    public CacheObject<TKey, TContent>? Tail { get; set; } = null;


    public MemoryCache(long sizeLimit, bool debugItems = false)
    {
        if(sizeLimit < 1)
            throw new ArgumentException("Size limit must be greater than 0");
        
        _sizeLimit = sizeLimit;
        _debugItems = debugItems;
    }

    public void Add(CacheObject<TKey, TContent> item)
    {
        
        SetHead(item);
        
        //get existing item
        if(Items.TryGetValue(item.Key, out var existingItem))
        {
            UpdateExistingItem(item,existingItem);
        }
        else // add item
        {
            Items.Add(item.Key, item);
            
            if (Tail is null)
            {
                Tail = item;
            }
            
            if(Items.Count > _sizeLimit)
            {
                Items.Remove(Tail.Key);
                Tail = Tail.Previous;
                if (Tail != null) Tail.Next = null;
            }
        }

    
        if(_debugItems)
        {
            PrintItems();
        }
    }
    
    public CacheObject<TKey, TContent>? Get(TKey key)
    {
        if(_debugItems)
        {
            PrintItems();
        }
        
        if (Items.TryGetValue(key, out var item))
        {
            if (item != null)
            {
                RemoveItem(item);
                SetHead(item);
            }
        }
        else
        {
            return null;
        }
        return item;
    }
    
    public void Clear()
    {
        Items.Clear();
        Head = null;
        Tail = null;
    }

    private void SetHead(CacheObject<TKey, TContent> item)
    {
        item.Next = Head;
        if(Head != null)
        {
            Head.Previous = item;
        }
        Head = item;
    }

    private void PrintItems()
    {
        Console.WriteLine("Current items in cache:");
        var item = Head;
        while (item != null)
        {
            Console.WriteLine(item.Key);
            item = item.Next;
        }
        Console.WriteLine("----------------------");
    }
    
    private void UpdateExistingItem(CacheObject<TKey, TContent> item, CacheObject<TKey, TContent>? existingItem)
    {
        if(existingItem != null)
        {
            Items[item.Key] = item;
            existingItem.Content = item.Content;
            RemoveItem(existingItem);
        }
    }
    
    private void RemoveItem(CacheObject<TKey, TContent> item)
    {
        if(item.Previous != null)
        {
            item.Previous.Next = item.Next;
        }
        if(item.Next != null)
        {
            item.Next.Previous = item.Previous;
        }
        else
        {
            Tail = item.Previous;
        }
    }
}