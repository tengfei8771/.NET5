﻿using IRepository;
using IServices;
using IServices.ResModel;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqlSugarAndEntity.DataTransferObject.role.RoleDTO;

namespace Services
{
    public class RoleService:BaseService<roleinfo>, IRoleService
    {
        private IRoleRepository roleRepository;
        private IUserRepository userRepository;

        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository) :base(roleRepository)
        {
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
        }
        public ResponseModel GetUserByRoleID(decimal RoleID)
        {
            return CreateResponseModel(userRepository.GetUserByRole, RoleID);
        }

        public ResponseModel RoleForMenu(RoleForMenu dto)
        {
            List<rolemapmenu> list = new List<rolemapmenu>();
            foreach(decimal menuID in dto.MenuID)
            {
                rolemapmenu item = new rolemapmenu()
                {
                    MenuID = menuID,
                    RoleID = dto.RoleID
                };
                list.Add(item);
            }
            return CreateResponseModel(roleRepository.RoleForMenu, dto.RoleID, list);
        }

        public ResponseModel RoleForUser(RoleForUser dto)
        {
            List<usermaprole> list = new List<usermaprole>();
            foreach (decimal userid in dto.UserID)
            {
                usermaprole item = new usermaprole()
                {
                    RoleID =dto.RoleID,
                    UserID= userid
                };
                list.Add(item);
            }
            return CreateResponseModel(roleRepository.RoleForUser, dto.RoleID, list);
        }
    }
}