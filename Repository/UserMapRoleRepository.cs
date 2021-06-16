using IRepository;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserMapRoleRepository: BaseRepository<usermaprole>,IUserMapRoleRepository
    {
        private IBaseMethod _baseMethod;
        public UserMapRoleRepository(IBaseMethod baseMethod):base(baseMethod)
        {
            _baseMethod = baseMethod;
        }
    }
}
