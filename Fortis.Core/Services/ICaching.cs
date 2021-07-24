namespace KnowBe4.Core.Services
{
    public interface ICachingProvider
    {
        bool AddCache(string key, object value,int expireinmins = 0);
        bool RemoveCache(string key);
        object GetCacheValue(string key);
        bool SetCacheValue(string key, object value,int expireinmins);
        bool CacheKeyExist(string key);
    }    
}