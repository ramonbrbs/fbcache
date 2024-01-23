using Finbourne.Cache.MemoryCache;
using FluentAssertions;

namespace Finbourne.Cache.Memory.Tests;

[TestFixture]
public class MemoryCacheTests
{
    [Test]
    public void AddThreeItems_WhenSizeIsTwo_ShouldKeepTwoItems()
    {
        var cache = new MemoryCache<int,string>(2, true);
        cache.Add(new CacheObject<int, string>(1, "one"));
        cache.Add(new CacheObject<int, string>(2, "two"));
        cache.Add(new CacheObject<int, string>(3, "three"));
        cache.Items.Should().HaveCount(2);
        cache.Head.Content.Should().Be("three");
    }
    
    [Test]
    public void Add_WhenRemovedItem_ShouldAddItemWithNewContent()
    {
        var cache = new MemoryCache<int,string>(2, true);
        cache.Add(new CacheObject<int, string>(1, "one"));
        cache.Add(new CacheObject<int, string>(2, "two"));
        cache.Add(new CacheObject<int, string>(3, "three"));
        cache.Add(new CacheObject<int, string>(1, "one_updated"));
        
        cache.Items.Should().HaveCount(2);
        cache.Head.Content.Should().Be("one_updated");
        cache.Tail.Content.Should().Be("three");
    }

    [Test]
    public void Add_WhenExistingItem_ShouldKeepItemsAndUpdateContent()
    {
        var cache = new MemoryCache<int,string>(3, true);
        cache.Add(new CacheObject<int, string>(1, "one"));
        cache.Add(new CacheObject<int, string>(2, "two"));
        cache.Add(new CacheObject<int, string>(3, "three"));
        cache.Add(new CacheObject<int, string>(1, "one_updated"));
        cache.Add(new CacheObject<int, string>(3, "two_updated"));
        
        cache.Items.Should().HaveCount(3);
        cache.Head.Content.Should().Be("two_updated");
        cache.Tail.Content.Should().Be("two");
    }

    [Test]
    public void Add_WhenSizeIsOne_ShouldKeepLastItem()
    {
        var cache = new MemoryCache<int,string>(1, true);
        cache.Add(new CacheObject<int, string>(1, "one"));
        cache.Add(new CacheObject<int, string>(2, "two"));
        cache.Add(new CacheObject<int, string>(3, "three"));
        
        cache.Items.Should().HaveCount(1);
        cache.Head.Content.Should().Be("three");
        cache.Tail.Content.Should().Be("three");
    }

    
    public class ComplexObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    [Test]
    public void Add_ComplexObjects_ShouldKeepItsCorrectValues()
    {
        var objOne = new ComplexObject { Id = 1, Name = "one" };
        var objTwo = new ComplexObject { Id = 2, Name = "two" };
        var objThree = new ComplexObject { Id = 3, Name = "three" };
        
        var cache = new MemoryCache<int,ComplexObject>(2, true);
        cache.Add(new CacheObject<int, ComplexObject>(1, objOne));
        cache.Add(new CacheObject<int, ComplexObject>(2, objTwo));
        cache.Add(new CacheObject<int, ComplexObject>(3, objThree));
        
        cache.Items.Should().HaveCount(2);
        cache.Head.Content.Should().Be(objThree);
        cache.Tail.Content.Should().Be(objTwo);
    }
}