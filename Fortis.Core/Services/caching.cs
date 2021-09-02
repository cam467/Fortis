namespace Fortis.Core.Services
{
    using System;
    using System.Runtime.Caching;

    public class MemoryCacher : ICachingProvider
    {
        public bool AddCache(string key, object value,int expireinmins = 0)
        {
            return SetCacheValue(key,value,expireinmins);
        }

        public bool RemoveCache(string key)
        {
            var cache = MemoryCache.Default;
            var res = cache.Remove(key);
            return res != null;
        }

        public object GetCacheValue(string key)
        {
            var cache = MemoryCache.Default;
            return cache.Get(key);
        }

        public bool SetCacheValue(string key, object value,int expireinmins)
        {
            CacheItem item = new CacheItem(key,value);
            var cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();
            if (expireinmins != 0) policy.SlidingExpiration = TimeSpan.FromMinutes(expireinmins);
            try
            {
                cache.Set(item,policy);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CacheKeyExist(string key)
        {
            var cache = MemoryCache.Default;
            return cache.Contains(key);
        }
    }
}