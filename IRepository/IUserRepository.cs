
using SqlSugarAndEntity;
using SqlSugarAndEntity.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IRepository
{
    public interface IUserRepository:IBaseRepository<userinfo> 
    {
        List<UserinfoBusinessModel> GetUserByRole(decimal RoleID);

        List<UserinfoBusinessModel> GetUserInfo(Expression<Func<userinfo, usermaporg,orginfo, bool>> WhereExp,int page,int limit,ref int total);

        void UpdateUserInfo(userinfo user, List<usermaporg> map);

        void DeleteUserinfo(Expression<Func<userinfo, bool>> UserExp, Expression<Func<usermaporg, bool>> MapExp);
    }
}
