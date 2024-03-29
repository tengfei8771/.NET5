﻿using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugarAndEntity;
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
    [ApiExplorerSettings(GroupName = "base_api")]
    public class OrgController : ControllerBase
    {
        private IOrgService orgService;
        public OrgController(IOrgService orgService)
        {
            this.orgService = orgService;
        }
        /// <summary>
        /// 获取懒加载的组织机构树节点
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetLazyOrgTree(decimal? ParentId, int page = 1, int limit = 1000) => Ok(orgService.GetLazyOrgTree(ParentId, page, limit));
        /// <summary>
        /// 获取整个组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOrgTree")]
        public IActionResult GetOrgTree() => Ok(orgService.GetOrgTree());
        /// <summary>
        /// 创建组织机构节点
        /// </summary>
        /// <param name="orginfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateOrg([FromBody] orginfo orginfo) => Ok(orgService.Insert(orginfo));
        /// <summary>
        /// 修改组织结构节点
        /// </summary>
        /// <param name="orginfo"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateOrg([FromBody] orginfo orginfo) => Ok(orgService.Update(orginfo, t => new { t.ID }));
        /// <summary>
        /// 删除组织机构节点
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteOrg(decimal ID) => Ok(orgService.Delete(t => t.ID == ID));
    }
}
