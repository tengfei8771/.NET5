using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Common;

namespace WebApi.Controllers
{
    [JwtVerification]
    [Route("[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private IMenuService menuService;
        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }
        /// <summary>
        /// 根据用户ID获取允许用户获得的菜单
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        [HttpGet("GetMenubyRole")]
        public IActionResult GetMenubyRole(decimal userId)
            => Ok(menuService.GetMenubyRole(userId));
    }
}
