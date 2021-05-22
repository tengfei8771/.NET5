using IServices.ResModel;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface ILoginService:IBaseService<userinfo>
    {
        ResponseModel Login(string account, string password);
    }
}
