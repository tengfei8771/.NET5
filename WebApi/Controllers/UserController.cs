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
        [AllowAnyJwtVerification]
        [HttpGet("login")]
        public IActionResult Login(string Account,string pwd)
        {
            return Ok(userService.Login(Account, pwd));
        }

    }
}
