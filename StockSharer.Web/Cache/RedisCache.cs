using System;
using System.Runtime.Caching;
using StackExchange.Redis;

namespace StockSharer.Web.Cache
{
    public class RedisCache
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisCache()
        {
            _connectionMultiplexer = MemoryCache.Default.Get("RedisConnection") as ConnectionMultiplexer;
            if (_connectionMultiplexer != null) return;
            _connectionMultiplexer = ConnectionMultiplexer.Connect("stocksharer.wsdxan.0001.euw1.cache.amazonaws.com");
            MemoryCache.Default.Set("RedisConnection", _connectionMultiplexer, ObjectCache.InfiniteAbsoluteExpiration);
        }

        public void Set(string key, string item, TimeSpan expiry)
        {
            var database = _connectionMultiplexer.GetDatabase();
            database.StringSet(key, item, expiry);
        }

        public string Get(string key)
        {
            var database = _connectionMultiplexer.GetDatabase();
            return database.StringGet(key);
        }
    }
}