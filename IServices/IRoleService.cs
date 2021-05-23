using IServices.ResModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqlSugarAndEntity.DataTransferObject.role.RoleDTO;

namespace IServices
{
    public interface IRoleService
    {
        /// <summary>
        /// 给角色赋予菜单权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseModel RoleForMenu(RoleForMenu dto);
        /// <summary>
        /// 给用户赋予角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseModel RoleForUser(RoleForUser dto);
        /// <summary>
        /// 根据权限获取权限下的用户
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        ResponseModel GetUserByRoleID(decimal RoleID);
    }
}
