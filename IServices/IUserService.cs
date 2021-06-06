using IServices.ResModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace IServices
{
    public interface IUserService:IBaseService<userinfo>
    {
        ResponseModel ImportUserinfo(Stream s);
        ResponseModel ExportUserinfo(Expression<Func<userinfo,bool>> WhereCondition);
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ResponseModel Login(string account, string password);
        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ResponseModel RefreshToken(JObject value);
    }
}
