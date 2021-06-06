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

    }
}
