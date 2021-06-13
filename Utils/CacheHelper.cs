using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class CacheHelper
    {
        private static IMemoryCache Cache= new MemoryCache(Options.Create(new MemoryCacheOptions()));
        /// <summary>
        /// 根据键获取缓存内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetObjectKey(object key)
        {
            return Cache.Get(key);
        }
        /// <summary>
        /// 根据键获取缓存内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetTByKey<T>(object key)
        {
            return Cache.Get<T>(key);
        }
        /// <summary>
        /// 缓存item 永不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool SetValue<T>(object key, T item)
        {
            Cache.Set(key, item);
            return Exists(key);
        }
        /// <summary>
        /// 根据key缓存内容，并设置相对过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="exptime"></param>
        public static bool SetValue<T>(object key,T item,DateTimeOffset exptime)
        {
            Cache.Set(key, item, exptime);
            return Exists(key);
        }
        /// <summary>
        /// 根据key缓存内容，并设置绝对期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存的key</param>
        /// <param name="item">缓存的值</param>
        /// <param name="absoluteExpirationRelativeToNow">绝对过期时间</param>
        /// <param name="isSliding">是否滑动过期(设置为true会导致命中缓存继续延长设置的时间)</param>
        /// <returns></returns>
        public static bool SetValue<T>(object key, T item, TimeSpan absoluteExpirationRelativeToNow,bool isSliding = false)
        {
            Cache.Set(key, item, isSliding?new MemoryCacheEntryOptions().SetSlidingExpiration(absoluteExpirationRelativeToNow): new MemoryCacheEntryOptions().SetAbsoluteExpiration(absoluteExpirationRelativeToNow));
            return Exists(key);
        }
        public static bool GetOrCreate<T>(object key,T item,int ExpSecond, bool isSliding = false)
        {
            Cache.GetOrCreate(key, entity =>
            {
                if (Exists(key))
                {
                    return GetTByKey<T>(key);
                }
                else
                {
                    entity.Value = item;
                    if (isSliding)
                    {
                        entity.SetAbsoluteExpiration(TimeSpan.FromSeconds(ExpSecond));
                    }
                    else
                    {
                        entity.SetSlidingExpiration(TimeSpan.FromSeconds(ExpSecond));
                    }
                    return item;
                }
            });
            return Exists(key);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public static bool Remove(object key)
        {
            if(Exists(key))
            {
                Cache.Remove(key);
            }
            return Exists(key);
        }

        public static List<object> GetCacheKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = Cache.GetType().GetField("_entries", flags).GetValue(Cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<object>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key);
            }
            return keys;
        }
        /// <summary>
        /// 获取是否含有key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool TryGetT<T>(object key,out T res)
        {
            return Cache.TryGetValue(key, out res);
        }

        public static bool Exists(object key)
        {
            return Cache.TryGetValue(key, out object res);
        }

    }
}
