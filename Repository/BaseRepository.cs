using IRepository;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private IBaseMethod _baseMethod;
        public BaseRepository(IBaseMethod baseMethod)
        {
            _baseMethod = baseMethod;
        }
        public bool Delete(T entity)
        {
            return _baseMethod.Db().Deleteable(entity).ExecuteCommand() > 0;
        }
        public bool Delete(Expression<Func<T, bool>> WhereExp)
        {
            return _baseMethod.Db().Deleteable<T>().Where(WhereExp).ExecuteCommand()>0;
        }
        public void DeleteAll(List<T> list)
        {
            _baseMethod.Db().Deleteable(list).ExecuteCommand();
        }

        public void ExecuteSqlCommand(string sql)
        {
            throw new NotImplementedException();
        }

        public DataTable FromSql(string sql)
        {
            return _baseMethod.Db().Ado.GetDataTable(sql);
        }

        public List<T> GetInfo(Expression<Func<T, bool>> predicate)
        {       
            return _baseMethod.Db().Queryable<T>().Where(predicate).ToList();
        }

        public List<T> GetInfoByPage(Expression<Func<T, bool>> predicate, int page, int limit, ref int total)
        {
            return _baseMethod.Db().Queryable<T>().Where(predicate).ToPageList(page,limit,ref total);
        }
        public List<T> GetOrderbyInfo(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy)
        {
            return _baseMethod.Db().Queryable<T>().Where(predicate).OrderBy(orderBy).ToList();
        }

        public List<T> GetOrderbyInfoByPage(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, int page, int limit, ref int total)
        {
            return _baseMethod.Db().Queryable<T>().Where(predicate).PartitionBy(orderBy).ToPageList(page, limit, ref total);
        }

        public DataTable GetInfoToDataTable(Expression<Func<T, bool>> predicate)
        {
            return _baseMethod.Db().Queryable<T>().Where(predicate).ToDataTable();
        }
        public DataTable GetInfoToDataTableByPage(Expression<Func<T, bool>> predicate, int page, int limit, ref int total)
        {
            return _baseMethod.Db().Queryable<T>().Where(predicate).ToDataTablePage(page,limit,ref total);
        }
        public bool Insert(T entity)
        {
            return _baseMethod.Db().Insertable(entity).ExecuteCommand()>0;
        }

        public void InsertAll(List<T> list)
        {
            _baseMethod.Db().Insertable(list).ExecuteCommand();
        }

        public void InsertMany<T1>(T entityT, T1 eneityT1)
        {
            var db = _baseMethod.Db();
            try
            {
                db.Ado.BeginTran();
                db.Insertable(entityT).ExecuteCommand();
                db.Insertable(entityT).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch(Exception e)
            {
                db.Ado.RollbackTran();
                throw new Exception(e.Message);
            }
        }

        public void InsertMany<T1>(List<T> listT, List<T1> listT1)
        {
            var db = _baseMethod.Db();
            try
            {
                db.Ado.BeginTran();
                db.Insertable(listT).ExecuteCommand();
                db.Insertable(listT1).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception e)
            {
                db.Ado.RollbackTran();
                throw new Exception(e.Message);
            }
        }

        public bool Update(T entity)
        {
            return _baseMethod.Db().Updateable(entity).ExecuteCommand() > 0;
        }
        public bool Update(T entity,Expression<Func<T,object>> WhereExp)
        {
            return _baseMethod.Db().Updateable(entity).WhereColumns(WhereExp).ExecuteCommand() > 0;
        }
        public bool Update(Expression<Func<T, T>> SetColumns, Expression<Func<T, object>> WhereExp)
        {
            return _baseMethod.Db().Updateable(SetColumns).WhereColumns(WhereExp).ExecuteCommand() > 0;
        }
        public bool UpdateIgnoreColumns(T entity, Expression<Func<T, object>> WhereExp,Expression<Func<T,object>>IgnoreExpress)
        {
            return _baseMethod.Db().Updateable(entity).IgnoreColumns(IgnoreExpress).WhereColumns(WhereExp).ExecuteCommand() > 0;
        }
        public bool UpdateAppiontColumns(T entity, Expression<Func<T, object>> WhereExp, Expression<Func<T, object>> UpdateExpress)
        {
            return _baseMethod.Db().Updateable(entity).UpdateColumns(UpdateExpress).WhereColumns(WhereExp).ExecuteCommand() > 0;
        }

        public bool UpdateAll(List<T> list)
        {
            return _baseMethod.Db().Updateable(list).ExecuteCommand()>0;
        }
        public bool UpdateAll(List<T> list, Expression<Func<T, object>> WhereExp)
        {
            return _baseMethod.Db().Updateable(list).WhereColumns(WhereExp).ExecuteCommand()>0;
        }


    }
}
