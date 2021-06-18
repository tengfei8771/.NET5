using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Serilog;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlSugarAndEntity
{
    public static class DataBaseConfig
    {
        public static ConnectionConfig _config = null;
        static DataBaseConfig()
        {
            GetConfig();
        }
        private static void GetConfig()
        {
            var _builder = new ConfigurationBuilder();
            var config = _builder.Add(new JsonConfigurationSource { Path = "DBConfig.json", Optional = false, ReloadOnChange = true }).Build();
            _config = new ConnectionConfig();
            _config.ConnectionString = config.GetSection($"MasterConnetion").Value;
            _config.IsAutoCloseConnection = true;
            string DBType = config.GetSection("DBType").Value.ToUpper();
            int SlaveCount = Convert.ToInt32(config.GetSection("SlaveCount").Value);
            switch (DBType)
            {
                case "SQLSERVER":
                    _config.DbType = DbType.SqlServer;
                    break;
                case "MYSQL":
                    _config.DbType = DbType.MySql;
                    break;
                case "ORACLE":
                    _config.DbType = DbType.Oracle;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (SlaveCount > 0)
            {
                _config.SlaveConnectionConfigs = new List<SlaveConnectionConfig>();
                for(int i=0;i< SlaveCount; i++)
                {
                    SlaveConnectionConfig slaveConnectionConfig = new SlaveConnectionConfig()
                    {
                        HitRate = config.GetSection($"SlaveConnetions:{i}").GetValue<int>("HitRate"),
                        ConnectionString = config.GetSection($"SlaveConnetions:{i}").GetValue<string>("ConnectionString")
                    };
                    _config.SlaveConnectionConfigs.Add(slaveConnectionConfig);
                }
            }
            _config.AopEvents = new AopEvents()
            {
                OnLogExecuting = (sql, p) =>
                {
                    Log.Logger.Information(SqlSguarExtensionMethod.LookSQL(sql, p));
                    //Console.WriteLine(SqlSguarExtensionMethod.LookSQL(sql, p));
                }
            };

            //扩展方法
            _config.ConfigureExternalServices = new ConfigureExternalServices()
            {
                SqlFuncServices = CreateExternal(),
                DataInfoCacheService = new SugarCache()
            };
        }

        private static List<SqlFuncExternal> CreateExternal()
        {
            var expMethods = new List<SqlFuncExternal>();
            #region group_concat方法
            SqlFuncExternal GroupConcat = new SqlFuncExternal()
            {
                UniqueMethodName = "GroupConcat",
                MethodValue = (expInfo, dbType, expContext) =>
                {
                    if (dbType == DbType.MySql)
                    {
                        if (expInfo.Args.Count == 1)
                        {
                            return string.Format("group_concat({0})", expInfo.Args[0].MemberName);
                        }
                        else
                        {
                            throw new Exception("未实现");
                        }
                    }
                    else if (dbType == DbType.SqlServer)
                    {
                        if (expInfo.Args.Count == 1)
                        {
                            return string.Format("string_agg({0},',')", expInfo.Args[0].MemberName);
                        }
                        else
                        {
                            throw new Exception("未实现");
                        }
                    }
                    else if (dbType == DbType.Oracle)
                    {
                        if (expInfo.Args.Count == 1)
                        {
                            return string.Format("wm_concat({0})", expInfo.Args[0].MemberName);
                        }
                        else
                        {
                            throw new Exception("未实现");
                        }
                    }
                    else
                    {
                        throw new Exception("未实现");
                    }
                       
                }
            };
            expMethods.Add(GroupConcat);
            return expMethods;
            #endregion
        }
    }
}
