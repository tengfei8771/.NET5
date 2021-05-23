
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IMenuRepository:IBaseRepository<menuinfo>
    {
        List<menuinfo> GetMenubyRole(decimal userId);
        List<menuinfo> GetLazyMenuTreeNode(decimal? ParentMenuID);
    }
}
