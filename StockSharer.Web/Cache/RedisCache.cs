﻿using System;
using System.Net;
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
            var configuration =  new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    EndPoints = { new DnsEndPoint("stocksharer.wsdxan.0001.euw1.cache.amazonaws.com", 6379) }
                };
            _connectionMultiplexer = ConnectionMultiplexer.Connect(configuration);
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