using IBaseRepository;
using IServices;
using IServices.ResModel;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static SqlSugarAndEntity.DataTransferObject.role.RoleDTO;

namespace Services
{
    public class RoleService:BaseService<roleinfo>, IRoleService
    {
        private IRoleRepository roleRepository;
        private IUserRepository userRepository;
        private IBaseRepository<usermaprole> userMapRoleRepository;

        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository, IBaseRepository<usermaprole> userMapRoleRepository) :base(roleRepository)
        {
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
            this.userMapRoleRepository = userMapRoleRepository;
        }


        public ResponseModel DeleteRole(decimal RoleID)
        {
            return CreateResponseModel<Expression<Func<roleinfo, bool>>, Expression<Func<rolemapmenu, bool>>>(roleRepository.DeleteRole, t => t.ID == RoleID, y => y.RoleID == RoleID);
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
            int total = 0;
            //先获取这个权限下所有的用户信息用来判断传过来的用户ID是否已经被赋权过了
            var usersHasRoled=userRepository.GetRoleAuthorized(dto.RoleID, 1, 500,ref total);
            foreach (decimal userid in dto.UserID)
            {
                var user = usersHasRoled.Where(t => t.ID == userid).FirstOrDefault();
                //没赋权过生成实体
                if (user != null)
                {
                    continue;
                }
                else
                {
                    usermaprole item = new usermaprole()
                    {
                        RoleID = dto.RoleID,
                        UserID = userid
                    };
                    list.Add(item);
                    
                }
            }
            return CreateResponseModel(roleRepository.RoleForUser, dto.RoleID, list);
        }
        public ResponseModel CancelRoleForUser(RoleForUser dto)
        {
            return CreateResponseModel<Expression<Func<usermaprole,bool>>>(userMapRoleRepository.Delete, t => t.RoleID == dto.RoleID && dto.UserID.Contains(t.UserID));
        }

    }
}
