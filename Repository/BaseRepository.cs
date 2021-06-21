using IBaseRepository;
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
        public virtual bool Delete(T entity)
        {
            return _baseMethod.Db().Deleteable(entity).ExecuteCommand() > 0;
        }
        public virtual bool Delete(Expression<Func<T, bool>> WhereExp)
        {
            return _baseMethod.Db().Deleteable<T>().Where(WhereExp).ExecuteCommand()>0;
        }
        public virtual void Delete(List<T> list)
        {
            _baseMethod.Db().Deleteable(list).ExecuteCommand();
        }

        public virtual void ExecuteSqlCommand(string sql)
        {
            throw new NotImplementedException();
        }

        public virtual DataTable FromSql(string sql)
        {
            return _baseMethod.Db().Ado.GetDataTable(sql);
        }

        public virtual List<T> GetInfo(Expression<Func<T, bool>> predicate)
        {       
            return _baseMethod.Db().Queryable<T>().WhereIF(predicate!=null, predicate).ToList();
        }

        public virtual List<T> GetInfoByPage(Expression<Func<T, bool>> predicate, int page, int limit,ref int total)
        {
            return _baseMethod.Db().Queryable<T>().WhereIF(predicate!=null, predicate).ToPageList(page,limit,ref total);
        }
        public virtual List<T> GetOrderbyInfo(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy)
        {
            return _baseMethod.Db().Queryable<T>().WhereIF(predicate != null, predicate).OrderBy(orderBy).ToList();
        }

        public virtual List<T> GetOrderbyInfoByPage(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, int page, int limit,ref int total)
        {
            return _baseMethod.Db().Queryable<T>().WhereIF(predicate != null, predicate).PartitionBy(orderBy).ToPageList(page, limit, ref total);
        }

        public virtual DataTable GetInfoToDataTable(Expression<Func<T, bool>> predicate)
        {
            return _baseMethod.Db().Queryable<T>().WhereIF(predicate != null, predicate).ToDataTable();
        }
        public virtual DataTable GetInfoToDataTableByPage(Expression<Func<T, bool>> predicate, int page, int limit,ref int total)
        {
            return _baseMethod.Db().Queryable<T>().WhereIF(predicate != null, predicate).ToDataTablePage(page,limit,ref total);
        }
        public virtual bool Insert(T entity)
        {
            return _baseMethod.Db().Insertable(entity).ExecuteCommand()>0;
        }

        public virtual void Insert(List<T> list)
        {
            _baseMethod.Db().Insertable(list).ExecuteCommand();
        }

        public virtual void InsertMany<T1>(T entityT, T1 eneityT1)
            where T1:class,new()
        {
            var db = _baseMethod.Db();
            try
            {
                db.Ado.BeginTran();
                db.Insertable(entityT).ExecuteCommand();
                db.Insertable(eneityT1).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch(Exception e)
            {
                db.Ado.RollbackTran();
                throw new Exception(e.Message);
            }
        }
        public virtual void InsertMany<T1>(T entityT, List<T1> listT1)
            where T1 : class, new()
        {
            var db = _baseMethod.Db();
            try
            {
                db.Ado.BeginTran();
                db.Insertable(entityT).ExecuteCommand();
                db.Insertable(listT1).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception e)
            {
                db.Ado.RollbackTran();
                throw new Exception(e.Message);
            }
        }

        public virtual void InsertMany<T1>(List<T> listT, List<T1> listT1)
            where T1 : class, new()
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

        public virtual bool Update(T entity)
        {
            return _baseMethod.Db().Updateable(entity).ExecuteCommand() > 0;
        }
        public virtual bool Update(T entity,Expression<Func<T,object>> WhereSelect)
        {
            return _baseMethod.Db().Updateable(entity).WhereColumns(WhereSelect).ExecuteCommand() > 0;
        }
        public virtual bool Update(Expression<Func<T, T>> SetColumns, Expression<Func<T, object>> WhereSelect)
        {
            return _baseMethod.Db().Updateable(SetColumns).WhereColumns(WhereSelect).ExecuteCommand() > 0;
        }
        public virtual bool UpdateIgnoreColumns(T entity, Expression<Func<T, object>> WhereSelect, Expression<Func<T,object>>IgnoreExpress)
        {
            return _baseMethod.Db().Updateable(entity).IgnoreColumns(IgnoreExpress).WhereColumns(WhereSelect).ExecuteCommand() > 0;
        }
        public virtual bool UpdateAppiontColumns(T entity, Expression<Func<T, object>> WhereSelect, Expression<Func<T, object>> UpdateExpress)
        {
            return _baseMethod.Db().Updateable(entity).UpdateColumns(UpdateExpress).WhereColumns(WhereSelect).ExecuteCommand() > 0;
        }

        public virtual bool Update(List<T> list)
        {
            return _baseMethod.Db().Updateable(list).ExecuteCommand()>0;
        }
        public virtual bool Update(List<T> list, Expression<Func<T, object>> WhereExp)
        {
            return _baseMethod.Db().Updateable(list).WhereColumns(WhereExp).ExecuteCommand()>0;
        }


    }
}
