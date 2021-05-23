﻿using IRepository;
using IServices;
using IServices.ResModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public ResponseModel DeleteAll(List<T> list)
        {
            return CreateResponseModel(baseRepository.DeleteAll, list);
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

        public ResponseModel InsertAll(List<T> list)
        {
            return CreateResponseModel(baseRepository.InsertAll, list);
        }

        public ResponseModel InsertMany<T1>(T entityT, T1 eneityT1)
        {
            return CreateResponseModel(baseRepository.InsertMany, entityT, eneityT1);
        }

        public ResponseModel InsertMany<T1>(List<T> listT, List<T1> listT1)
        {
            return CreateResponseModel(baseRepository.InsertMany, listT, listT1);
        }

        public ResponseModel Update(T entity)
        {
            return CreateResponseModel(baseRepository.Update, entity);
        }

        public ResponseModel UpdateAll(List<T> list)
        {
            return CreateResponseModel(baseRepository.UpdateAll, list);
        }
        #region 新增，修改，删除执行委托操作通用方法
        protected ResponseModel CreateResponseModel<P>(Action<P> func, P p)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                func.Invoke(p);
                res.Code = 2000;
                res.Message = "成功";
            }
            catch (Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
            }
            return res;
        }
        
        protected ResponseModel CreateResponseModel<P, P1>(Action<P, P1> func, P p, P1 p1)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                func.Invoke(p, p1);
                res.Code = 2000;
                res.Message = "成功";
            }
            catch (Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
            }
            return res;
        }

        protected ResponseModel CreateResponseModel<P>(Func<P, bool> func, P p)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                bool Success = func.Invoke(p);
                if (Success)
                {
                    res.Code = 2000;
                    res.Message = "成功";
                }
                else
                {
                    res.Code = -1;
                    res.Message = "失败";
                }
            }
            catch (Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
            }
            return res;
        }
        #endregion
        #region 不进行分页的的查询的委托通用方法
        protected ResponseModel CreateResponseModel<P, PResult>(Func<P, PResult> func, P p)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult obj = func.Invoke(p);
                res.Code = 2000;
                res.Message = "成功";
                res.Items = obj;
            }
            catch (Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
            }
            return res;
        }

        protected ResponseModel CreateResponseModel<P, P1, PResult>(Func<P, P1, PResult> func, P p, P1 p1)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult obj = func.Invoke(p, p1);
                res.Code = 2000;
                res.Message = "成功";
                res.Items = obj;
            }
            catch (Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
            }
            return res;
        }


        protected ResponseModel CreateResponseModel<P, P1, P2, PResult>(Func<P, P1, P2, PResult> func, P p, P1 p1, P2 p2)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult obj = func.Invoke(p, p1, p2);
                res.Code = 2000;
                res.Message = "成功";
                res.Items = obj;
                res.Total = p2 as int?;
            }
            catch (Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
            }
            return res;
        }


        protected ResponseModel CreateResponseModel<P, P1, P2, P3, PResult>(Func<P, P1, P2, P3, PResult> func, P p, P1 p1, P2 p2, P3 p3)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult obj = func.Invoke(p, p1, p2, p3);
                res.Code = 2000;
                res.Message = "成功";
                res.Items = obj;
                
            }
            catch (Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
            }
            return res;
        }
        protected ResponseModel CreateResponseModel<P, P1, P2, P3, P4, PResult>(Func<P, P1, P2, P3, P4, PResult> func, P p, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                PResult obj = func.Invoke(p, p1, p2, p3, p4);
                res.Code = 2000;
                res.Message = "成功";
                res.Items = obj;
                
            }
            catch (Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
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
                res.Code = 2000;
                res.Message = "成功";
                res.Total = p2 as int?;
            }
            catch (Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
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
                res.Code = 2000;
                res.Message = "成功";
                res.Total = p3 as int?;
            }
            catch(Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
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
                res.Code = 2000;
                res.Message = "成功";
                res.Total = p4 as int?;
            }
            catch (Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
            }
            return res;
        }
    }
}