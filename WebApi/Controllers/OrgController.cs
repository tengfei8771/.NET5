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
    [Route("api/[controller]")]
    [ApiController]
    [JwtVerification]
    public class OrgController : ControllerBase
    {
        private IOrgService orgService;
        public OrgController(IOrgService orgService)
        {
            this.orgService = orgService;
        }
    }
}
