using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Common;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [JwtVerification]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [AllowAnyJwtVerification]
        [HttpGet("login")]
        public IActionResult Login(string Account,string pwd)
        {
            return Ok(userService.Login(Account, pwd));
        }
        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        [AllowAnyJwtVerification]
        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken([FromBody]JObject value) => Ok(userService.RefreshToken(value));
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="Name">用户姓名</param>
        /// <param name="UserAccount">用户账号</param>
        /// <param name="UserPhone">用户电话</param>
        /// <param name="IdNumber">用户ID</param>
        /// <param name="OrgName">用户组织机构名称</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserInfo(string Name, string UserAccount, string UserPhone, string IdNumber, string OrgName, int page, int limit)
            => Ok(userService.GetUserInfo(Name, UserAccount, UserPhone, IdNumber, OrgName, page, limit));

    }
}
