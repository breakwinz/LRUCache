# LRUCache
A Least Recently Used In Memory Cache written in .NET7

Requirements:
.NET 7 https://dotnet.microsoft.com/en-us/download/dotnet/7.0

Total work time: ~4 hours.

An in-memory cache written in .NET 7, only required nuget package is Xunit. 
Written with Singleton pattern in mind. Uses locking objects for thread safety. Notify events are raised outside of thread locks. 
Capacity is determined by number of items in cache, not by memory/cache size. 
Can in theory store as large objects as possible until OOM is raised. No protection against OOM.

Note! The cache will store values as type Object. Consumer will need to cast the retrieved item back to their type. 
Consumer should ideally store a map with Keys they've sent to cache along with the type.

Alternate implementation could be considered where cache is initialized with Tkey and TValue where consumer defines the type of key and value. 
However, this restricts consumer to only upload objects of a specific type. Arguable which implementation is better, its about use-case.

Note that there is a single lock object for the entire cache, i.e the lock will lock both read and write operations.
This could be further enhanced by introducing a read-lock and a write-lock, however further consideration would have to be taken to the Set method.
This is because one thread could potentially be updating a cacheitem while another is reading it or vice versa. The Set method allows replacing existing keys in cache.

Linked List is used for tracking the least recently used items in cache. Upside: dont need to store a "Last Accessed/Used" variable along the CacheItem, 
also dont need to do an .OrderBy(LastUsed) upon checking for evicting an item. This saves operation time, sorting algorithm would be O(log N) in best case scenario and O(n^2) in worst case, by using something like
a bubble or merge sort.

A linked list saves us cpu-time by always keeping the Cache in order. However, downsides, if the cache is loaded with a very large amount of small item, then the memory overhead with using a linked list could be noticable on memory footprint of cache.
