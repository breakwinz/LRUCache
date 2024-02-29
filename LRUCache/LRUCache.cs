using LRUCache.EventArgs;

namespace LRUCache;

public class LRUCache : ILRUCache
{
    /// <summary>
    /// Capacity of Cache
    /// </summary>
    private int _capacity;

    /// <summary>
    /// CacheMap for fast key and CacheItem lookup.
    /// </summary>
    private Dictionary<string, CacheItem> _cacheMap = new();

    /// <summary>
    /// LinkedList to store CacheNodes in Least Recently Used order.
    /// Reordering is fast, O(1) operation.
    /// </summary>
    private LinkedList<CacheItem> _lruList = new();

    /// <summary>
    /// Lock Object for Thread Safety
    /// </summary>
    private object _lockObject = new();

    /// <summary>
    /// Subscriber event. Get notified if an Item is Evicted.
    /// </summary>
    public event EventHandler<ItemEvictedEventArgs> ItemEvicted;

    /// <summary>
    /// Least Recently Used cache.
    /// </summary>
    /// <param name="capacity">The capacity of the cache</param>
    /// <exception cref="ArgumentException">Raised if capacity is less than or equal to 0</exception>
    public LRUCache(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than 0", nameof(capacity));

        _capacity = capacity;
    }

    /// <summary>
    /// Adds new item to cache of any arbitrary object type.
    /// If item with Key already exists, the cache item will be replaced with new value.
    /// </summary>
    /// <param name="key">The key of the cache item</param>
    /// <param name="value">The value (object) to cache</param>
    public void Set(string key, object value)
    {
        ItemEvictedEventArgs evictedItemArgs = null;

        lock (_lockObject)
        {
            if (_cacheMap.TryGetValue(key, out CacheItem node))
            {
                node.Value = value;
                _lruList.Remove(node.ListNode);
                _lruList.AddFirst(node.ListNode);
            }
            else
            {
                if (_cacheMap.Count >= _capacity)
                {
                    evictedItemArgs = EvictLeastRecentlyUsed();
                }

                var newNode = new CacheItem(key, value);
                _lruList.AddFirst(newNode.ListNode);
                _cacheMap[key] = newNode;
            }
        }

        // Raise the event outside of the lock
        if (evictedItemArgs != null)
        {
            OnItemEvicted(evictedItemArgs);
        }
    }

    /// <summary>
    /// Looks up and gets an item in cache based on a Key
    /// </summary>
    /// <param name="key">The Key to get cache item</param>
    /// <returns>cacheItem of type Object</returns>
    /// <exception cref="KeyNotFoundException">If key is not found in cache</exception>
    public object? Get(string key)
    {
        lock (_lockObject)
        {
            if (_cacheMap.TryGetValue(key, out CacheItem node))
            {
                _lruList.Remove(node.ListNode);
                _lruList.AddFirst(node.ListNode);
                return node.Value;
            }

            throw new KeyNotFoundException($"The key {key} was not found in the cache.");
        }
    }

    /// <summary>
    /// Removes an item from cache.
    /// </summary>
    /// <param name="key">The key to remove from cache along with its item.</param>
    public void Remove(string key)
    {
        lock (_lockObject)
        {
            if (_cacheMap.TryGetValue(key, out CacheItem node))
            {
                _lruList.Remove(node.ListNode);
                _cacheMap.Remove(key);

                return;
            }

            throw new KeyNotFoundException($"The key {key} was not found in the cache.");
        }
    }

    /// <summary>
    /// Evicts the least recently used item from cache.
    /// </summary>
    /// <returns>ItemEvictedEventArgs to be used for a notify event. Null if there is nothing evicted</returns>
    private ItemEvictedEventArgs EvictLeastRecentlyUsed()
    {
        if (_lruList.Last != null)
        {
            var lruNode = _lruList.Last.Value;
            _lruList.RemoveLast();
            _cacheMap.Remove(lruNode.Key);

            return new ItemEvictedEventArgs(lruNode.Key, lruNode.Value);
        }
        return null;
    }

    protected virtual void OnItemEvicted(ItemEvictedEventArgs e)
    {
        EventHandler<ItemEvictedEventArgs> handler = ItemEvicted;
        handler?.Invoke(this, e);
    }
}



