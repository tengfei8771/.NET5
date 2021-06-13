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
        /// 通过t=>new {t.ID}的形式指定where条件更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="WhereSelect">形似t=>new {t.ID}</param>
        /// <returns></returns>
        bool Update(T entity, Expression<Func<T, object>> WhereSelect);
        /// <summary>
        /// 根据表达式更新实体
        /// </summary>
        /// <param name="SetColumns">需要更新的字段 形似 t=>new T(){UpdateColumn=1}</param>
        /// <param name="WhereSelect">where表达式 形似t=>new {t.ID}</param>
        /// <returns></returns>
        bool Update(Expression<Func<T,T>> SetColumns, Expression<Func<T, object>> WhereSelect);
        /// <summary>
        /// 通过t=>t.new T(){Field="1"}的形式指定where条件和忽略更新的字段
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="WhereSelect">形似t=>new {t.ID}</param>
        /// <param name="IgnoreExpress">形似t=>new {t.IgnoreColumn}</param>
        /// <returns></returns>
        bool UpdateIgnoreColumns(T entity, Expression<Func<T, object>> WhereSelect, Expression<Func<T, object>> IgnoreExpress);
        /// <summary>
        /// 通过t=>t.new T(){Field="1"}的形式指定where条件和只更新的字段
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="WhereSelect">形似t=>new {t.ID}</param>
        /// <param name="UpdateExpress">形似t=>new {t.UpdateColumn}</param>
        /// <returns></returns>
        bool UpdateAppiontColumns(T entity, Expression<Func<T, object>> WhereSelect, Expression<Func<T, object>> UpdateExpress);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(T entity);
        /// <summary>
        /// 根据表达式删除数据
        /// </summary>
        /// <param name="WhereExp"></param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> WhereExp);
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
        void Insert(List<T> list);
        /// <summary>
        /// 批量删除list
        /// </summary>
        /// <param name="list">实体list</param>
        void Delete(List<T> list);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        bool Update(List<T> list);
        /// <summary>
        /// 批量更新 指定更新条件
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="WhereExp">形似t=>new {t.ID}</param>
        /// <returns></returns>
        bool Update(List<T> list, Expression<Func<T, object>> WhereExp);
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
        DataTable GetInfoToDataTableByPage(Expression<Func<T, bool>> predicate, int page, int limit,ref int total);
    }
}
