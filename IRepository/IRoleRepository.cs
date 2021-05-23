using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IRoleRepository:IBaseRepository<roleinfo>
    {
        void RoleForMenu(decimal RoleId, List<rolemapmenu> map);

        void RoleForUser(decimal RoleId, List<usermaprole> map);
    }
}
