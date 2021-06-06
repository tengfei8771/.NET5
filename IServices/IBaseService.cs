using IServices.ResModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IBaseService<T> where T:class,new()
    {
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ResponseModel Insert(T entity);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ResponseModel Update(T entity);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ResponseModel Delete(T entity);
        /// <summary>
        /// 根据表达式删除数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ResponseModel Delete(Expression<Func<T,bool>> expression);
        /// <summary>
        /// 根据查询条件获取全部数据
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <returns></returns>
        ResponseModel GetInfo(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据查询表达式和排序表达式获取数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        ResponseModel GetOrderbyInfo(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy);
        /// <summary>
        /// 根据查询条件获取分页后的数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        ResponseModel GetInfoByPage(Expression<Func<T, bool>> predicate, int page, int limit);
        /// <summary>
        /// 根据查询条件和排序条件获取分页后的数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        ResponseModel GetOrderbyInfoByPage(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, int page, int limit);
        /// <summary>
        /// 获取执行sql后的结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        ResponseModel FromSql(string sql);
        /// <summary>
        /// 获取执行sql后的结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        ResponseModel ExecuteSqlCommand(string sql);
        /// <summary>
        /// 插入实体list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        ResponseModel InsertAll(List<T> list);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        ResponseModel DeleteAll(List<T> list);
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        ResponseModel UpdateAll(List<T> list);
        /// <summary>
        /// 插入多个实体
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="entityT"></param>
        /// <param name="eneityT1"></param>
        /// <returns></returns>
        ResponseModel InsertMany<T1>(T entityT, T1 eneityT1);
        /// <summary>
        /// 插入多个实体list
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="listT"></param>
        /// <param name="listT1"></param>
        /// <returns></returns>
        ResponseModel InsertMany<T1>(List<T> listT, List<T1> listT1);
        /// <summary>
        /// 根据查询条件获取datatable
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        ResponseModel GetInfoToDataTable(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据查询条件获取分页后的datatable
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        ResponseModel GetInfoToDataTableByPage(Expression<Func<T, bool>> predicate, int page, int limit);
    }
}
