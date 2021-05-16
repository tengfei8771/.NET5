using IServices.ResModel;
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
    }
}
