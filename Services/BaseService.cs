using IRepository;
using IServices;
using IServices.ResModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utils;
using static Utils.JwtHelper;

namespace Services
{
    public class BaseService<T> : IBaseService<T> where T : class, new()
    {
        private IBaseRepository<T> baseRepository;
        public BaseService(IBaseRepository<T> baseRepository)
        {
            this.baseRepository = baseRepository;
        }
        public ResponseModel Delete(T entity)
        {
            return CreateResponseModel(baseRepository.Delete, entity);
        }
        public ResponseModel Delete(Expression<Func<T,bool>> Exp)
        {
            return CreateResponseModel(baseRepository.Delete, Exp);
        }
        public ResponseModel Delete(List<T> list)
        {
            return CreateResponseModel(baseRepository.Delete, list);
        }

        public ResponseModel ExecuteSqlCommand(string sql)
        {
            return CreateResponseModel(baseRepository.ExecuteSqlCommand, sql);
        }

        public ResponseModel FromSql(string sql)
        {
            return CreateResponseModel(baseRepository.FromSql, sql);
        }

        public ResponseModel GetInfo(Expression<Func<T, bool>> predicate)
        {
            return CreateResponseModel(baseRepository.GetInfo, predicate);
        }

        public ResponseModel GetInfoByPage(Expression<Func<T, bool>> predicate, int page, int limit)
        {
            int total = 0;
            var model = CreateResponseModelByPage(baseRepository.GetInfoByPage, predicate, page, limit, ref total);
            return model;
        }

        public ResponseModel GetInfoToDataTable(Expression<Func<T, bool>> predicate)
        {
            return CreateResponseModel(baseRepository.GetInfoToDataTable, predicate);
        }

        public ResponseModel GetInfoToDataTableByPage(Expression<Func<T, bool>> predicate, int page, int limit)
        {
            int total = 0;
            return CreateResponseModelByPage(baseRepository.GetInfoToDataTableByPage, predicate, page, limit,ref total);
        }

        public ResponseModel GetOrderbyInfo(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy)
        {
            return CreateResponseModel(baseRepository.GetOrderbyInfo, predicate, orderBy);
        }

        public ResponseModel GetOrderbyInfoByPage(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, int page, int limit)
        {
            int total = 0;
            return CreateResponseModelByPage(baseRepository.GetOrderbyInfoByPage, predicate, orderBy, page, limit,ref total);
        }

        public ResponseModel Insert(T entity)
        {
            return CreateResponseModel(baseRepository.Insert, entity);
        }

        public ResponseModel Insert(List<T> list)
        {
            return CreateResponseModel(baseRepository.Insert, list);
        }

        public ResponseModel InsertMany<T1>(T entityT, T1 eneityT1) 
            where T1 : class, new()
        {
            return CreateResponseModel(baseRepository.InsertMany, entityT, eneityT1);
        }

        public ResponseModel InsertMany<T1>(List<T> listT, List<T1> listT1)
            where T1 : class, new()
        {
            return CreateResponseModel(baseRepository.InsertMany, listT, listT1);
        }

        public ResponseModel Update(T entity)
        {
            return CreateResponseModel(baseRepository.Update, entity);
        }
        public ResponseModel Update(T entity, Expression<Func<T, object>> WhereExp)
        {
            return CreateResponseModel(baseRepository.Update, entity,WhereExp);
        }

        public ResponseModel Update(Expression<Func<T, T>> SetColumns, Expression<Func<T, object>> WhereExp)
        {
            return CreateResponseModel(baseRepository.Update, SetColumns, WhereExp);
        }

        public ResponseModel UpdateIgnoreColumns(T entity, Expression<Func<T, object>> WhereExp, Expression<Func<T, object>> IgnoreExpress)
        {
            return CreateResponseModel(baseRepository.UpdateIgnoreColumns, entity, WhereExp, IgnoreExpress);
        }

        public ResponseModel UpdateAppiontColumns(T entity, Expression<Func<T, object>> WhereExp, Expression<Func<T, object>> UpdateExpress)
        {
            return CreateResponseModel(baseRepository.UpdateAppiontColumns, entity, WhereExp, UpdateExpress);
        }

        public ResponseModel Update(List<T> list)
        {
            return CreateResponseModel(baseRepository.Update, list);
        }
        #region 新增，修改，删除执行委托操作通用方法
        /// <summary>
        /// 一个参数的无返回值模型包装方法,一般用于事务批量，返回的code是操作成功类
        /// </summary>
        /// <typeparam name="P">实体</typeparam>
        /// <param name="func">委托执行的方法</param>
        /// <param name="p">参数p</param>
        /// <returns></returns>
        protected ResponseModel CreateResponseModel<P>(Action<P> func, P p)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                func.Invoke(p);
                res.code = (int)ResponseType.OperationSucess;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.OperationSucess);
            }
            catch (Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }
        /// <summary>
        /// 一个参数的无返回值模型包装方法,一般用于事务批量，返回的code是操作成功类
        /// </summary>
        /// <typeparam name="P">实体1</typeparam>
        /// <typeparam name="P1">实体2</typeparam>
        /// <param name="func">委托执行的方法</param>
        /// <param name="p">实体1的实参</param>
        /// <param name="p1">实体2的实参</param>
        /// <returns></returns>
        protected ResponseModel CreateResponseModel<P, P1>(Action<P, P1> func, P p, P1 p1)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                func.Invoke(p, p1);
                res.code = (int)ResponseType.OperationSucess;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.OperationSucess);
            }
            catch (Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }
        /// <summary>
        /// 返回bool类型的返回模型包装方法,一般用于操作数据库数据的方法
        /// </summary>
        /// <typeparam name="P">实体1</typeparam>
        /// <param name="func">委托执行的方法</param>
        /// <param name="p">实参1</param>
        /// <returns></returns>
        protected ResponseModel CreateResponseModel<P>(Func<P, bool> func, P p)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                bool Success = func.Invoke(p);
                if (Success)
                {
                    res.code = (int)ResponseType.OperationSucess;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.OperationSucess);
                }
                else
                {
                    res.code = (int)ResponseType.OperationFail;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.OperationFail);
                }
            }
            catch (Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }
        /// <summary>
        /// 构造一个执行委托返回值为bool的包装模型,一般用于操作数据库
        /// </summary>
        /// <typeparam name="P">参数1类型</typeparam>
        /// <typeparam name="P1">参数2类型</typeparam>
        /// <param name="func">执行的委托方法</param>
        /// <param name="p">实参1</param>
        /// <param name="p1">实参2</param>
        /// <returns></returns>
        protected ResponseModel CreateResponseModel<P,P1>(Func<P,P1, bool> func, P p,P1 p1)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                bool Success = func.Invoke(p,p1);
                if (Success)
                {
                    res.code = (int)ResponseType.OperationSucess;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.OperationSucess);
                }
                else
                {
                    res.code = (int)ResponseType.OperationFail;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.OperationFail);
                }
            }
            catch (Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }
        protected ResponseModel CreateResponseModel<P, P1, P2>(Func<P, P1, P2, bool> func, P p, P1 p1, P2 p2)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                bool Success = func.Invoke(p, p1, p2);
                if (Success)
                {
                    res.code = (int)ResponseType.OperationSucess;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.OperationSucess);
                }
                else
                {
                    res.code = (int)ResponseType.OperationFail;
                    res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.OperationFail);
                }
               
            }
            catch (Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }
        #endregion
        #region 不进行分页的的查询的委托通用方法
        /// <summary>
        /// 一个参数的包装模型返回方法
        /// </summary>
        /// <typeparam name="P">实体</typeparam>
        /// <typeparam name="PResult">返回结果</typeparam>
        /// <param name="func">执行的委托</param>
        /// <param name="p">实参</param>
        /// <returns></returns>
        protected ResponseModel CreateResponseModel<P, PResult>(Func<P, PResult> func, P p)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult obj = func.Invoke(p);
                res.code = (int)ResponseType.GetInfoSucess;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.GetInfoSucess);
                res.items = obj;
            }
            catch (Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }

        protected ResponseModel CreateResponseModel<P, P1, PResult>(Func<P, P1, PResult> func, P p, P1 p1)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult obj = func.Invoke(p, p1);
                res.code = (int)ResponseType.GetInfoSucess;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.GetInfoSucess);
                res.items = obj;
            }
            catch (Exception e)
            {
                res.code =(int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }

        /// <summary>
        /// 执行三个参数的查询方法并将返回结果包装
        /// </summary>
        /// <typeparam name="P">参数1的类型</typeparam>
        /// <typeparam name="P1">参数2的类型</typeparam>
        /// <typeparam name="P2">参数3的类型</typeparam>
        /// <typeparam name="PResult">返回值类型</typeparam>
        /// <param name="func">委托执行的方法</param>
        /// <param name="p">实参1</param>
        /// <param name="p1">实参2</param>
        /// <param name="p2">实参3</param>
        /// <returns></returns>
        protected ResponseModel CreateResponseModel<P, P1, P2, PResult>(Func<P, P1, P2, PResult> func, P p, P1 p1, P2 p2)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult obj = func.Invoke(p, p1, p2);
                res.code = (int)ResponseType.GetInfoSucess;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.GetInfoSucess);
                res.items = obj;
            }
            catch (Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }


        protected ResponseModel CreateResponseModel<P, P1, P2, P3, PResult>(Func<P, P1, P2, P3, PResult> func, P p, P1 p1, P2 p2, P3 p3)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult obj = func.Invoke(p, p1, p2, p3);
                res.code = (int)ResponseType.GetInfoSucess;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.GetInfoSucess);
                res.items = obj;
                
            }
            catch (Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }
        protected ResponseModel CreateResponseModel<P, P1, P2, P3, P4, PResult>(Func<P, P1, P2, P3, P4, PResult> func, P p, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult obj = func.Invoke(p, p1, p2, p3, p4);
                res.code = (int)ResponseType.GetInfoSucess;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.GetInfoSucess);
                res.items = obj;
                
            }
            catch (Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }
        #endregion
        #region 声明最后一个参数是ref的委托
        protected delegate PResult LastPamaraRefDelegate<PResult, P, P1, P2>(P p, P1 p1, ref P2 p2);
        protected delegate PResult LastPamaraRefDelegate<PResult, P, P1, P2, P3>(P p, P1 p1, P2 p2, ref P3 p3);
        protected delegate PResult LastPamaraRefDelegate<PResult, P, P1, P2, P3, P4>(P p, P1 p1, P2 p2, P3 p3, ref P4 p4);
        #endregion
        protected ResponseModel CreateResponseModelByPage<PResult, P, P1, P2>
            (LastPamaraRefDelegate<PResult, P, P1, P2> func, P p, P1 p1,ref P2 p2)
            where P2 : struct
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult pResult = func.Invoke(p, p1,ref p2);
                res.items = pResult;
                res.code = (int)ResponseType.GetInfoSucess;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.GetInfoSucess);
                res.total = p2 as int?;
            }
            catch (Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }
        protected ResponseModel CreateResponseModelByPage<PResult, P, P1, P2, P3>
            (LastPamaraRefDelegate<PResult, P, P1, P2, P3> func,P p,P1 p1,P2 p2,ref P3 p3) 
            where P3 :struct
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult pResult = func.Invoke(p, p1, p2, ref p3);
                res.items = pResult;
                res.code = (int)ResponseType.GetInfoSucess;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.GetInfoSucess);
                res.total = p3 as int?;
            }
            catch(Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }

        protected ResponseModel CreateResponseModelByPage<PResult, P, P1, P2, P3,P4>
            (LastPamaraRefDelegate<PResult, P, P1, P2, P3,P4> func, P p, P1 p1, P2 p2, P3 p3,ref P4 p4)
            where P3 : struct
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult pResult = func.Invoke(p, p1, p2, p3,ref p4);
                res.items = pResult;
                res.code = (int)ResponseType.GetInfoSucess;
                res.message = ReflectionConvertHelper.GetEnumDescription(ResponseType.GetInfoSucess);
                res.total = p4 as int?;
            }
            catch (Exception e)
            {
                res.code = (int)ResponseType.Exception;
                res.message = e.Message;
            }
            return res;
        }

        
    }
}
