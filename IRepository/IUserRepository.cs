
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepository
{
    public interface IUserRepository:IBaseRepository<userinfo> 
    {
        List<userinfo> GetUserByRole(decimal RoleID);
    }
}
