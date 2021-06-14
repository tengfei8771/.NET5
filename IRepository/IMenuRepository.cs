
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqlSugarAndEntity.DataTransferObject.role.RoleDTO;

namespace IRepository
{
    public interface IMenuRepository:IBaseRepository<menuinfo>
    {
        List<menuinfo> GetMenuByUserId(decimal userId);
        List<menuinfo> GetLazyMenuTreeNode(decimal? ParentMenuID,int page,int limit,ref int total);
        DataTable GetMenuTree();
        RoleForMenu GetMenuByRoleId(decimal roleId);
    }
}
