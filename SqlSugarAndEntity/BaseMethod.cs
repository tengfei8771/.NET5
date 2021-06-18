using Serilog;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarAndEntity
{
    public class BaseMethod : IBaseMethod
    {
        private SqlSugarClient GetClient() 
        {
            SqlSugarClient db = new SqlSugarClient(DataBaseConfig._config);
            db.Aop.OnLogExecuted = (sql, p) =>
            {
                Log.Logger.Information($"SqlExecuteTime:[{db.Ado.SqlExecutionTime}]------------------");
                //Console.WriteLine($"SqlExecuteTime:[{db.Ado.SqlExecutionTime}]------------------");
            };
            return db;
        } 

        public ISqlSugarClient Db() => GetClient();

        public IAdo Sql() => GetClient().Ado;
    }
}
