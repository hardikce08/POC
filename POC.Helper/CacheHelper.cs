using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Caching;

namespace POC.Helper
{
    /// <summary>
    /// Cache Management Gateway
    /// 2 Caching patterns implemented, the generic IIS in memory cache and AppFabric cahche.
    /// AppFabric is the primary use case.
    /// 
    /// The cache is split into regions for easier data identification and purging capability.
    /// </summary>
    public interface IGenericCache
    {
        void Add<T>(string key, T value, CacheRegionEnum region, int durationInMinutes = 30);
        void Remove(string key, CacheRegionEnum region);
        bool Get<T>(string key, out T value, CacheRegionEnum region);
        void Update<T>(string key, T value, CacheRegionEnum region, int durationInMinutes = 30);
        bool Exists(string key, CacheRegionEnum region);
        void ClearRegion(CacheRegionEnum region);
        int RegionInfo(CacheRegionEnum region);
        List<string> GetKeys(CacheRegionEnum region);

        List<string> GetAllKeys();

        void ClearAllRegion();

        string GetCacheNameToGetOnlineUsers();
    }
    public enum CacheRegionEnum
    {
        Common,
        Security,
    }

    public static class CacheHelper
    {
        private static IGenericCache cache;

        public static IGenericCache GetCache
        {
            get
            {
                if (cache == null)
                {

                    cache = new GSSInMemoryCache();
                    cache.Exists("test", CacheRegionEnum.Common);

                }
                return cache;
            }
        }

        public static void Add<T>(T value, CacheRegionEnum region, int durationInMinutes, string keyformat, params object[] args)
        {
            GetCache.Add<T>(string.Format(keyformat, args), value, region, durationInMinutes);
        }

        public static void Add<T>(string key, T value, CacheRegionEnum region, int durationInMinutes = 30)
        {
            GetCache.Add<T>(key, value, region, durationInMinutes);
        }

        public static void Remove(string key, CacheRegionEnum region)
        {
            GetCache.Remove(key, region);
        }

        public static void Remove(string keyformat, CacheRegionEnum region, params object[] args)
        {
            GetCache.Remove(string.Format(keyformat, args), region);
        }

        public static bool Get<T>(out T value, CacheRegionEnum region, string keyformat, params object[] args)
        {
            return GetCache.Get<T>(string.Format(keyformat, args), out value, region);
        }

        public static bool Get<T>(string key, out T value, CacheRegionEnum region)
        {
            return GetCache.Get<T>(key, out value, region);
        }

        public static void Update<T>(string key, T value, CacheRegionEnum region, int durationInMinutes = 30)
        {
            GetCache.Update(key, value, region, durationInMinutes);
        }

        public static bool Exists(string key, CacheRegionEnum region)
        {
            return GetCache.Exists(key, region);
        }

        public static bool Exists(string keyformat, CacheRegionEnum region, params object[] args)
        {
            return GetCache.Exists(string.Format(keyformat, args), region);
        }

        public static void ClearRegion(CacheRegionEnum region)
        {
            GetCache.ClearRegion(region);
        }

        public static int RegionInfo(CacheRegionEnum region)
        {
            return GetCache.RegionInfo(region);
        }
        public static List<string> GetKeys(CacheRegionEnum region)
        {
            return GetCache.GetKeys(region);
        }

        public static void ClearAllRegion()
        {
            GetCache.ClearAllRegion();
        }

        public static List<string> GetAllKeys()
        {
            return GetCache.GetAllKeys();
        }

        public static string GetCacheNameToGetOnlineUsers()
        {
            return GetCache.GetCacheNameToGetOnlineUsers();
        }

    }

    class GSSInMemoryCache : IGenericCache
    {
        static Dictionary<CacheRegionEnum, MemoryCache> _cache;
        private static Dictionary<CacheRegionEnum, MemoryCache> GetCache
        {
            get
            {
                if (_cache == null)
                {
                    _cache = new Dictionary<CacheRegionEnum, MemoryCache>();

                    var regions = Enum.GetNames(typeof(CacheRegionEnum));

                    foreach (var region in regions)
                    {
                        _cache.Add((CacheRegionEnum)Enum.Parse(typeof(CacheRegionEnum), region), new MemoryCache(region));
                        Trace.WriteLine(string.Format("Cache::Region Created::{0}", region));
                    }
                    Trace.WriteLine(string.Format("Cache::Initiated"));


                }
                return _cache;
            }
        }

        public void Add<T>(string key, T value, CacheRegionEnum region, int durationInMinutes = 30)
        {
            GetCache[region].Add(key, value, DateTime.UtcNow.AddMinutes(durationInMinutes), null);

            Trace.WriteLine(string.Format("Cache::{0}::Add::{1}", region, key));
        }

        public void Remove(string key, CacheRegionEnum region)
        {
            GetCache[region].Remove(key, null);

            Trace.WriteLine(string.Format("Cache::{0}::Remove::{1}", region, key));
        }

        public bool Get<T>(string key, out T value, CacheRegionEnum region)
        {
            try
            {
                if (!Exists(key, region))
                {
                    value = default(T);
                    return false;
                }
                value = (T)GetCache[region].Get(key, null);

                Trace.WriteLine(string.Format("Cache::{0}::Get::{1}", region, key));
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }

        public void Update<T>(string key, T value, CacheRegionEnum region, int durationInMinutes = 30)
        {
            GetCache[region].Add(key, value, DateTime.UtcNow.AddMinutes(durationInMinutes), null);

            Trace.WriteLine(string.Format("Cache::{0}::Update::{1}", region, key));
        }

        public bool Exists(string key, CacheRegionEnum region)
        {
            Trace.WriteLine(string.Format("Cache::{0}::Exists::{1}", region, key));

            return GetCache[region].Contains(key, null);
        }

        public void ClearRegion(CacheRegionEnum region)
        {
            GetCache[region] = new MemoryCache(region.ToString());
            Trace.WriteLine(string.Format("Cache::{0}::ClearRegion", region));
        }

        public int RegionInfo(CacheRegionEnum region)
        {
            return (int)GetCache[region].GetCount(null);
        }

        public List<string> GetKeys(CacheRegionEnum region)
        {
            return new List<string>(new[] { "InMemoryCannotParse" });
        }

        public List<string> GetAllKeys()
        {
            return new List<string>(new[] { "InMemoryCannotParse" });
        }

        public void ClearAllRegion()
        {

        }

        public string GetCacheNameToGetOnlineUsers()
        {
            return "";
        }
    }
    public static class CacheKeys
    {
        /// <summary>
        /// SessionProfile_{username}
        /// </summary>
        public static string SesssionProfile = "SessionProfile_{0}";
        public static string SesssionAdminProfile = "SessionAdminProfile_{0}";
    }
}
