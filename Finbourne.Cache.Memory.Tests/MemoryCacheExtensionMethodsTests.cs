using Finbourne.Cache.MemoryCache;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Core;

namespace Finbourne.Cache.Memory.Tests;

[TestFixture]
public class MemoryCacheExtensionMethodsTests
{
    [Test]
    public void GetFromCacheOrInsert_WhenItemIsNotInCache_ShouldAddItemToCache()
    {
        var cache = new MemoryCache<int, string?>(2);
        var list = new List<string> {"one", "two", "three"};
        var item = cache.GetFromCacheOrGetFirstOrDefault(1, list, s => s == "one");
        item.Should().Be("one");
        cache.Items.Should().HaveCount(1);
    }

    [Test]
    public void GetFromCacheOrInsert_WhenOnCache_ShouldNotHitResource()
    {
        
        var cache = new MemoryCache<int, string?>(2);
        var mockList = Substitute.For<IEnumerable<string>>();
        
        mockList.GetEnumerator().Returns(new List<string> {"one", "two", "three"}.GetEnumerator());
        
        cache.GetFromCacheOrGetFirstOrDefault(1, mockList, s => s == "one");
        cache.GetFromCacheOrGetFirstOrDefault(1, mockList, s => s == "one");

        mockList.Received(1).GetEnumerator();
        cache.Items.Should().HaveCount(1);
        cache.Head.Content.Should().Be("one");
    }
}