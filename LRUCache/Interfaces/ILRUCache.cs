using LRUCache.EventArgs;

namespace LRUCache;

public interface ILRUCache
{
    /// <summary>
    /// Subscriber event. Get notified if an Item is Evicted.
    /// </summary>
    event EventHandler<ItemEvictedEventArgs> ItemEvicted;

    /// <summary>
    /// Adds new item to cache of any arbitrary object type.
    /// If item with Key already exists, the cache item will be replaced with new value.
    /// </summary>
    /// <param name="key">The key of the cache item</param>
    /// <param name="value">The value (object) to cache</param>
    void Set(string key, object value);

    /// <summary>
    /// Looks up and gets an item in cache based on a Key
    /// </summary>
    /// <param name="key">The Key to get cache item</param>
    /// <returns>cacheItem of type Object</returns>
    /// <exception cref="KeyNotFoundException">If key is not found in cache</exception>
    object? Get(string key);

    /// <summary>
    /// Removes an item from cache.
    /// </summary>
    /// <param name="key">The key to remove from cache along with its item.</param>
    /// <returns>True if found and removed. False if key not found.</returns>
    void Remove(string key);
}