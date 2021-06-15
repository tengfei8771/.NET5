using AutoMapper;
using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SqlSugarAndEntity;
using SqlSugarAndEntity.DataTransferObject.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;
using WebApi.Common;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtVerification]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        private readonly IMapper mapper;
        private IConfiguration Configuration;
        public UserController(IUserService userService, IMapper mapper, IConfiguration Configuration)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.Configuration = Configuration;
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
        /// <summary>
        /// 创建用户信息
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateUserInfo([FromBody] UserDTO userDTO)
        {
            string PwdKey = Configuration.GetSection("AESKey").Value;
            List<usermaporg> maps = new List<usermaporg>();
            var userId= SnowflakeHelper.GetId();
            foreach (decimal orgid in userDTO.OrgId)
            {
                usermaporg map = new usermaporg()
                {
                    UserID = userId,
                    OrgID = orgid
                };
                maps.Add(map);
            }
            userinfo user= mapper.Map<userinfo>(userDTO);
            user.ID = userId;
            user.UserPassWord = AESHelper.AesEncrypt(user.UserPassWord, PwdKey);
            return Ok(userService.CreateUserInfo(user, maps));
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateUserInfo([FromBody] UserDTO userDTO)
        {
            string PwdKey = Configuration.GetSection("AESKey").Value;
            List<usermaporg> maps = new List<usermaporg>();
            foreach (decimal orgid in userDTO.OrgId)
            {
                usermaporg map = new usermaporg()
                {
                    UserID = userDTO.ID,
                    OrgID = orgid
                };
                maps.Add(map);
            }
            userinfo user = mapper.Map<userinfo>(userDTO);
            user.UserPassWord = AESHelper.AesEncrypt(user.UserPassWord, PwdKey);
            return Ok(userService.UpdateUserInfo(user, maps));
        }
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteUserInfo(decimal userId)
            => Ok(userService.DeleteUserInfo(a=>a.ID==userId,b=>b.UserID==userId));
        /// <summary>
        /// 根据角色ID获取被授权的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetRoleAuthorized")]
        public IActionResult GetRoleAuthorized(decimal roleId, int page, int limit)
            => Ok(userService.GetRoleAuthorized(roleId, page, limit));

        /// <summary>
        /// 根据角色ID获取未被授权的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetRoleNotAuthorized")]
        public IActionResult GetRoleNotAuthorized(decimal roleId, int page, int limit)
            => Ok(userService.GetRoleNotAuthorized(roleId, page, limit));





    }
}
