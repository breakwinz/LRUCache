
public class LRUCacheTests
{
    [Fact]
    public void SetAndGetItemAsString_ShouldRetrieveCorrectValue()
    {
        // Arrange
        var cache = new LRUCache.LRUCache(2);

        // Act
        cache.Set("key1", "value1");
        var value = cache.Get("key1");

        // Assert
        Assert.Equal("value1", value);
    }

    [Fact]
    public void SetAndGetItemAsInt_ShouldRetrieveCorrectValue()
    {
        // Arrange
        var cache = new LRUCache.LRUCache(2);

        // Act
        cache.Set("key1", 1);
        var value = cache.Get("key1");

        // Assert
        Assert.Equal(1, value);
    }

    [Fact]
    public void SetAndGetItemAsDictionary_ShouldRetrieveCorrectValue()
    {
        // Arrange
        var cache = new LRUCache.LRUCache(2);
        var dict = new Dictionary<string, string>();
        dict.Add("test1", "test2");

        // Act
        cache.Set("key1", dict);
        var value = cache.Get("key1");
        var castValue = (Dictionary<string, string>)value;

        // Assert
        Assert.Equal(dict["test1"], castValue["test1"]);
        Assert.Equal(dict, value);
    }

    [Fact]
    public void GetNonExistentKey_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var cache = new LRUCache.LRUCache(2);

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => cache.Get("nonexistentKey"));
    }

    [Fact]
    public void UpdateExistingKey_ShouldUpdateValue()
    {
        // Arrange
        var cache = new LRUCache.LRUCache(2);
        cache.Set("key1", "value1");

        // Act
        cache.Set("key1", "newValue");
        var value = cache.Get("key1");

        // Assert
        Assert.Equal("newValue", value);
    }

    [Fact]
    public void EvictLeastRecentlyUsedItem_WhenCapacityIsExceeded()
    {
        // Arrange
        var cache = new LRUCache.LRUCache(2);
        cache.Set("key1", "value1");
        cache.Set("key2", "value2");

        // Act
        cache.Set("key3", "value3"); // This should evict key1

        // Assert
        Assert.Throws<KeyNotFoundException>(() => cache.Get("key1"));
        Assert.Equal("value2", cache.Get("key2"));
        Assert.Equal("value3", cache.Get("key3"));
    }

    [Fact]
    public void RemoveItem_ShouldRemoveSuccessfully()
    {
        // Arrange
        var cache = new LRUCache.LRUCache(2);
        cache.Set("key1", "value1");

        // Act
        cache.Remove("key1");

        // Assert
        Assert.Throws<KeyNotFoundException>(() => cache.Get("key1"));
    }

    [Fact]
    public void RemoveNonExistentItem_ShouldThrowException()
    {
        // Arrange
        var cache = new LRUCache.LRUCache(2);

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => cache.Remove("nonexistentKey"));
    }

    [Fact]
    public void ItemEvictedEvent_ShouldBeRaised_WhenItemIsEvicted()
    {
        // Arrange
        var cache = new LRUCache.LRUCache(2);
        bool eventRaised = false;
        cache.ItemEvicted += (sender, args) =>
        {
            eventRaised = true;
            Assert.Equal("key1", args.Key);
            Assert.Equal("value1", args.Value);
        };

        cache.Set("key1", "value1");
        cache.Set("key2", "value2");

        // Act
        cache.Set("key3", "value3");

        // Assert
        Assert.True(eventRaised);
    }

    [Fact]
    public void ItemEvictedEvent_ShouldNotBeRaised_WhenNoItemIsEvicted()
    {
        // Arrange
        var cache = new LRUCache.LRUCache(3);
        bool eventRaised = false;
        cache.ItemEvicted += (sender, args) =>
        {
            eventRaised = true;
        };

        cache.Set("key1", "value1");
        cache.Set("key2", "value2");

        // Act
        cache.Set("key3", "value3");

        // Assert
        Assert.False(eventRaised);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Initialize_WithInvalidCapacity_ShouldThrowArgumentException(int invalidCapacity)
    {
        var exception = Assert.Throws<ArgumentException>(() => new LRUCache.LRUCache(invalidCapacity));
        Assert.Equal("Capacity must be greater than 0 (Parameter 'capacity')", exception.Message);
    }

    [Fact]
    public void SetAndGetItem_WithNullValue_DoesNotThrowAndStoresNull()
    {
        // Arrange
        var cache = new LRUCache.LRUCache(10);
        string testKey = "key1";

        // Act
        cache.Set(testKey, null);
        var cachedValue = cache.Get(testKey);

        // Assert
        Assert.Null(cachedValue);
    }
}