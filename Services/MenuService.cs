using IRepository;
using IServices;
using IServices.ResModel;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using static Utils.JwtHelper;

namespace Services
{
    public class MenuService:BaseService<menuinfo>,IMenuService
    {
        private IMenuRepository menuRepository;
        public MenuService(IMenuRepository menuRepository) : base(menuRepository)
        {
            this.menuRepository = menuRepository;
        }

        public ResponseModel GetLazyMenuTreeNode(decimal? ParentMenuID,int page,int limit)
        {
            int total = 0;
            return CreateResponseModelByPage(menuRepository.GetLazyMenuTreeNode, ParentMenuID,page,limit,ref total);
        }

        public ResponseModel GetMenuByRoleId(decimal roleId)
        {
            return CreateResponseModel(menuRepository.GetMenuByRoleId, roleId);
        }

        public ResponseModel GetMenuByUserId(decimal userId)
        {
            return CreateResponseModel(menuRepository.GetMenuByUserId, userId);
        }

        public ResponseModel GetMenuTree()
        {
            ResponseModel response = new ResponseModel();
            DataTable dt = menuRepository.GetMenuTree();
            response.items = ReflectionConvertHelper.ConvertDatatableToTreeList<menuinfo>(dt, "ID", "MenuParentID");
            return response;
        }
    }
}
