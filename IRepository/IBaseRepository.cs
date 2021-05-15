using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace IRepository
{
    public interface IBaseRepository<T> where T:class,new()
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool Insert(T entity);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(T entity);
        /// <summary>
        /// 根据表达式获取全部的数据
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <returns></returns>
        List<T> GetInfo(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据查询表达式和排序表达式获取全部数据
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        List<T> GetOrderbyInfo(Expression<Func<T, bool>> predicate,Expression<Func<T,object>>orderBy);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <param name="total">数据总数</param>
        /// <returns></returns>
        List<T> GetInfoByPage(Expression<Func<T, bool>> predicate,int page,int limit,ref int total);
        /// <summary>
        /// 获取排序后分页数据
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        List<T> GetOrderbyInfoByPage(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, int page, int limit,ref int total);
        /// <summary>
        /// 根据sql获取datatable
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns></returns>
        DataTable FromSql(string sql);
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql">执行的sql语句</param>
        void ExecuteSqlCommand(string sql);
        /// <summary>
        /// 批量插入list
        /// </summary>
        /// <param name="list">实体list</param>
        void InsertAll(List<T> list);
        /// <summary>
        /// 批量删除list
        /// </summary>
        /// <param name="list">实体list</param>
        void DeleteAll(List<T> list);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        void UpdateAll(List<T> list);
        /// <summary>
        /// 批量插入一个实体和另一个实体list
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="entityT"></param>
        /// <param name="eneityT1"></param>
        void InsertMany<T1>(T entityT, T1 eneityT1);
        /// <summary>
        /// 批量插入一个实体list和另一个实体list,一般用于导入
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="listT"></param>
        /// <param name="listT1"></param>
        void InsertMany<T1>(List<T> listT, List<T1> listT1);
        /// <summary>
        /// 根据查询表达式获取数据datable
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <returns></returns>
        DataTable GetInfoToDataTable(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据查询表达式获取分页后的数据
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <param name="total">总页数</param>
        /// <returns></returns>
        DataTable GetInfoToDataTableByPage(Expression<Func<T, bool>> predicate, int page, int limit, ref int total);
    }
}
