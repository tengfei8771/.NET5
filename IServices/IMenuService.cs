using IServices.ResModel;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IMenuService:IBaseService<menuinfo>
    {
        ResponseModel GetMenubyRole(decimal userId);

        ResponseModel GetLazyMenuTreeNode(decimal? ParentMenuID, int page, int limit);

        ResponseModel GetMenuTree();
    }
}
