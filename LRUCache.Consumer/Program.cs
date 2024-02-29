// See https://aka.ms/new-console-template for more information
using LRUCache;
using LRUCache.EventArgs;

class Program
{
    static void Main(string[] args)
    {
        // Initialize the LRUCache instance with a capacity of 5
        ILRUCache cache = new LRUCache.LRUCache(5);

        // Subscribe to the ItemEvicted event
        cache.ItemEvicted += Cache_ItemEvicted;

        // Populate the cache with some cacheItems
        cache.Set("key1", "value1");
        cache.Set("key2", "value2");
        cache.Set("key3", "value3");
        cache.Set("key4", "value4");
        cache.Set("key5", "value5");

        // Access some items to change their position in linkedlist
        Console.WriteLine($"Getting key3: {cache.Get("key3")}");
        Console.WriteLine($"Getting key1: {cache.Get("key1")}");

        // Add a new item, causing the least recently used item (key2) to be removed
        cache.Set("key6", "value6");

        // Try to get the removed item
        try
        {
            Console.WriteLine($"Getting key2: {cache.Get("key2")}");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
        }

        // Add another item just for fun.
        cache.Set("key7", "value7");
    }

    private static void Cache_ItemEvicted(object sender, ItemEvictedEventArgs e)
    {
        Console.WriteLine($"Item evicted with Key: {e.Key}, Value: {e.Value}");
    }
}
