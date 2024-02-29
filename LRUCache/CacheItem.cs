namespace LRUCache;

public class CacheItem
{
    public string Key { get; }
    public object Value { get; set; }
    public LinkedListNode<CacheItem> ListNode { get; }

    public CacheItem(string key, object value)
    {
        Key = key;
        Value = value;
        ListNode = new LinkedListNode<CacheItem>(this);
    }
}