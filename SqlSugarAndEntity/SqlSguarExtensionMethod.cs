using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SqlSugarAndEntity
{
    public static class SqlSguarExtensionMethod
    {
        /// <summary>
        /// 查看赋值后的sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars">参数</param>
        /// <returns></returns>
        public static string LookSQL(string sql, SugarParameter[] pars)
        {
            if (pars == null || pars.Length == 0) return sql;

            StringBuilder sb_sql = new StringBuilder(sql);
            var tempOrderPars = pars.Where(p => p.Value != null).OrderByDescending(p => p.ParameterName.Length).ToList();//防止 @par1错误替换@par12
            for (var index = 0; index < tempOrderPars.Count; index++)
            {
                sb_sql.Replace(tempOrderPars[index].ParameterName, "'" + tempOrderPars[index].Value.ToString() + "'");
            }
            return sb_sql.ToString();
        }

        public static string GroupConcat<T>(T t)
        {
            throw new NotSupportedException("Can only be used in expressions");
        }
    }
    public class SugarCache : ICacheService
    {
        public void Add<V>(string key, V value)
        {
            CacheHelper.SetValue(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey<V>(string key)
        {
            return CacheHelper.Exists(key);
        }

        public V Get<V>(string key)
        {
            return CacheHelper.GetTByKey<V>(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            throw new NotImplementedException();
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public void Remove<V>(string key)
        {
            throw new NotImplementedException();
        }
    }
}
