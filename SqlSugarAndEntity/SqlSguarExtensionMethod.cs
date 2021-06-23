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
            for (var i = pars.Length - 1; i >= 0; i--)
            {
                if (pars[i].DbType == System.Data.DbType.String
                    || pars[i].DbType == System.Data.DbType.DateTime
                    || pars[i].DbType == System.Data.DbType.Date
                    || pars[i].DbType == System.Data.DbType.Time
                    || pars[i].DbType == System.Data.DbType.DateTime2
                    || pars[i].DbType == System.Data.DbType.DateTimeOffset
                    || pars[i].DbType == System.Data.DbType.Guid
                    || pars[i].DbType == System.Data.DbType.VarNumeric
                    || pars[i].DbType == System.Data.DbType.AnsiStringFixedLength
                    || pars[i].DbType == System.Data.DbType.AnsiString
                    || pars[i].DbType == System.Data.DbType.StringFixedLength)
                {
                    sql = sql.Replace(pars[i].ParameterName, "'" + pars[i].Value?.ToString() + "'");
                }
                else if (pars[i].DbType == System.Data.DbType.Boolean)
                {
                    sql = sql.Replace(pars[i].ParameterName, Convert.ToBoolean(pars[i].Value) ? "1" : "0");
                }
                else
                {
                    sql = sql.Replace(pars[i].ParameterName, pars[i].Value?.ToString());
                }
            }

            return sql;
        }
        /// <summary>
        /// 格式化参数拼接成完整的SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static string ParameterFormat(string sql, object pars)
        {
            var param = (SugarParameter[])pars;
            return ParameterFormat(sql, param);
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
            CacheHelper.SetValue(key, value, TimeSpan.FromSeconds(cacheDurationInSeconds), true);
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
            return CacheHelper.GetCacheKeys().Select(t => t.ToString());
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (!CacheHelper.Exists(cacheKey))
            {
                var result = create.Invoke();
                CacheHelper.SetValue(cacheKey, result, TimeSpan.FromSeconds(cacheDurationInSeconds), true);
            }
            return CacheHelper.GetTByKey<V>(cacheKey);
        }

        public void Remove<V>(string key)
        {
            CacheHelper.Remove(key);
        }
    }
}
