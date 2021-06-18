using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Common;
using static SqlSugarAndEntity.DataTransferObject.role.RoleDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [JwtVerification]
    [ApiController]
    [ApiExplorerSettings(GroupName = "base_api")]
    public class RoleController : ControllerBase
    {
        private IRoleService roleService;
        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRole(int page,int limit) => Ok(roleService.GetInfoByPage(t => true,page,limit));
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateRole([FromBody] roleinfo roleinfo) => Ok(roleService.Insert(roleinfo));
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="roleinfo"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateRole([FromBody] roleinfo roleinfo) => Ok(roleService.Update(roleinfo, t => new { t.ID }));
        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteRole(decimal ID) => Ok(roleService.DeleteRole(ID));
        /// <summary>
        /// 角色赋予菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("RoleForMenu")]
        public IActionResult RoleForMenu([FromBody] RoleForMenu dto) => Ok(roleService.RoleForMenu(dto));
        /// <summary>
        /// 给用户赋予角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("RoleForUser")]
        public IActionResult RoleForUser([FromBody] RoleForUser dto) => Ok(roleService.RoleForUser(dto));

        /// <summary>
        /// 取消用户的角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("CancelRoleForUser")]
        public IActionResult CancelRoleForUser([FromBody] RoleForUser dto) => Ok(roleService.CancelRoleForUser(dto));

    }
}
