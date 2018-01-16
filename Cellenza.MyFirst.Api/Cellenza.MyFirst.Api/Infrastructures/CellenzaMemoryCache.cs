using System;
using Microsoft.Extensions.Caching.Memory;

namespace Cellenza.MyFirst.Api.Infrastructures
{
    public class CellenzaMemoryCache : ICache
    {
        private IMemoryCache memoryCache = new MemoryCache(new MemoryDistributedCacheOptions()
        {
            SizeLimit = null,
            ExpirationScanFrequency = TimeSpan.FromSeconds(10)
        });

        public void Insert<T>(string key, T clients)
        {
            memoryCache.Set(key, clients, TimeSpan.FromSeconds(30));
        }

        public bool TryGet<T>(string key, out T o)
        {
            if (memoryCache.TryGetValue(key, out object value))
            {
                o = (T)value;

                return true;
            }

            o = default(T);

            return false;
        }
    }

    public class CompositeCache : ICache
    {
        private readonly CellenzaMemoryCache memory;
        private readonly RedisCache redis;

        public CompositeCache(CellenzaMemoryCache memory, RedisCache redis)
        {
            this.memory = memory;
            this.redis = redis;
        }

        public void Insert<T>(string key, T clients)
        {
            this.memory.Insert(key, clients);
        }

        public bool TryGet<T>(string key, out T o)
        {
            if (memory.TryGet<T>(key, out var value1))
            {
                o = value1;
                return true;
            }

            if (redis.TryGet<T>(key, out var value2))
            {
                memory.Insert(key, value2);

                o = value2;
                return true;
            }

            o = default(T);
            return false;
        }
    }

    public class RedisCache : ICache
    {
        private IMemoryCache memoryCache = new MemoryCache(new MemoryDistributedCacheOptions()
        {
            SizeLimit = null,
            ExpirationScanFrequency = TimeSpan.FromSeconds(10)
        });

        public void Insert<T>(string key, T clients)
        {
            memoryCache.Set(key, clients, TimeSpan.FromSeconds(30));
        }

        public bool TryGet<T>(string key, out T o)
        {
            if (memoryCache.TryGetValue(key, out object value))
            {
                o = (T)value;

                return true;
            }

            o = default(T);

            return false;
        }
    }
}