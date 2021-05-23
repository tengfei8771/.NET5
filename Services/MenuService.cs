﻿using IRepository;
using IServices;
using IServices.ResModel;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MenuService:BaseService<menuinfo>,IMenuService
    {
        private IMenuRepository menuRepository;
        public MenuService(IMenuRepository menuRepository) : base(menuRepository)
        {
            this.menuRepository = menuRepository;
        }

        public ResponseModel GetLazyMenuTreeNode(decimal? ParentMenuID)
        {
            return CreateResponseModel(menuRepository.GetLazyMenuTreeNode, ParentMenuID);
        }

        public ResponseModel GetMenubyRole(decimal userId)
        {
            return CreateResponseModel(menuRepository.GetMenubyRole, userId);
        }
    }
}
