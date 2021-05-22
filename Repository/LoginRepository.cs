using IRepository;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class LoginRepository: BaseRepository<userinfo>, ILoginRepository
    {
        private IBaseMethod baseMethod;
        public LoginRepository(IBaseMethod baseMethod):base(baseMethod)
        {
            this.baseMethod = baseMethod;
            
        }
        public List<userinfo> GetLoginUserInfo(string account) => GetInfo(t => t.UserAccount == account);

    }
}
