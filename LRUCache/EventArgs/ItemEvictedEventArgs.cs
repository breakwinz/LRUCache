namespace LRUCache.EventArgs;

public class ItemEvictedEventArgs : System.EventArgs
{
    public string Key { get; }
    public object Value { get; }

    public ItemEvictedEventArgs(string key, object value)
    {
        Key = key;
        Value = value;
    }
}