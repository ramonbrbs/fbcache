﻿// See https://aka.ms/new-console-template for more information

using Finbourne.Cache.MemoryCache;

var cache = new MemoryCache<int,string>(2, true);
cache.Add(new CacheObject<int, string>(1, "one"));
cache.Add(new CacheObject<int, string>(2, "two"));
cache.Add(new CacheObject<int, string>(3, "three"));


var list = new List<string>(){"one", "two", "three"};

var item = cache.GetFromCacheOrGetFirstOrDefault(1, list, s => s == "one");