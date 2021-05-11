using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace IRepository
{
    public interface IBaseRepository<T> where T:class,new()
    {
        bool Insert(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        List<T> GetInfo(Expression<Func<T, bool>> predicate);
        List<T> GetOrderbyInfo(Expression<Func<T, bool>> predicate,Expression<Func<T,object>>orderBy);
        List<T> GetInfoByPage(Expression<Func<T, bool>> predicate,int page,int limit,ref int total);

        List<T> GetOrderbyInfoByPage(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, int page, int limit, ref int total);
        DataTable FromSql(string sql);
        void ExecuteSqlCommand(string sql);
        void InsertAll(List<T> list);

        void DeleteAll(List<T> list);
        void UpdateAll(List<T> list);

        void InsertMany<T1>(T entityT, T1 eneityT1);
        void InsertMany<T1>(List<T> listT, List<T1> listT1);

        DataTable GetInfoToDataTable(Expression<Func<T, bool>> predicate);
        DataTable GetInfoToDataTableByPage(Expression<Func<T, bool>> predicate, int page, int limit, ref int total);
    }
}
