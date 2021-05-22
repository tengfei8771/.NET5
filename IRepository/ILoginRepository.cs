
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface ILoginRepository:IBaseRepository<userinfo>
    {
        List<userinfo> GetLoginUserInfo(string account);
    }
}
