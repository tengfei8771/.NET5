using EFCore.BulkExtensions;
using Entity.Models;
using IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Repository

{
    //
    public class EFBaseRepository<T> :IBaseRepository<T> where T : class,new()
    {
        private readonly AppDBContext _appDBContext;

        public EFBaseRepository(AppDBContext _appDBContext)
        {
            this._appDBContext = _appDBContext;
        }
        public bool Delete(T entity)
        {
            _appDBContext.Remove(entity);
            return _appDBContext.SaveChanges()>0;
        }

        public bool Delete(Expression<Func<T, bool>> WhereExp)
        {
            throw new NotImplementedException();
        }

        public void Delete(List<T> list)
        {
            _appDBContext.BulkDelete(list);
        }

        public void ExecuteSqlCommand(string sql)
        {
            _appDBContext.Database.ExecuteSqlRaw(sql);
        }

        public DataTable FromSql(string sql)
        {
            throw new NotImplementedException();
            //return _appDBContext.Database.ExecuteSqlRaw(sql);
        }

        public List<T> GetInfo(Expression<Func<T, bool>> predicate)
        {
            return _appDBContext.Set<T>().Where(predicate).ToList();
        }

        public List<T> GetInfoByPage(Expression<Func<T, bool>> predicate, int page, int limit,ref int total)
        {
            throw new NotImplementedException();
        }

        public DataTable GetInfoToDataTable(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public DataTable GetInfoToDataTableByPage(Expression<Func<T, bool>> predicate, int page, int limit,ref int total)
        {
            throw new NotImplementedException();
        }

        public List<T> GetOrderbyInfo(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy)
        {
            throw new NotImplementedException();
        }

        public List<T> GetOrderbyInfoByPage(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, int page, int limit,ref int total)
        {
            throw new NotImplementedException();
        }

        public bool Insert(T entity)
        {
            _appDBContext.Add(entity);
            return _appDBContext.SaveChanges() > 0;
        }

        public void Insert(List<T> list)
        {
            _appDBContext.BulkInsert(list);
        }

        public void InsertMany<T1>(T entityT, T1 eneityT1)
        {
            throw new NotImplementedException();
        }

        public void InsertMany<T1>(List<T> listT, List<T1> listT1)
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity, Expression<Func<T, object>> WhereExp)
        {
            throw new NotImplementedException();
        }

        public bool Update(Expression<Func<T, T>> SetColumns, Expression<Func<T, object>> WhereExp)
        {
            throw new NotImplementedException();
        }

        public void Update(List<T> list)
        {
            throw new NotImplementedException();
        }

        public bool Update(List<T> list, Expression<Func<T, object>> WhereExp)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAppiontColumns(T entity, Expression<Func<T, object>> WhereExp, Expression<Func<T, object>> UpdateExpress)
        {
            throw new NotImplementedException();
        }

        public bool UpdateIgnoreColumns(T entity, Expression<Func<T, object>> WhereExp, Expression<Func<T, object>> IgnoreExpress)
        {
            throw new NotImplementedException();
        }

        bool IBaseRepository<T>.Update(List<T> list)
        {
            throw new NotImplementedException();
        }
    }
}
