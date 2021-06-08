﻿
using IServices;
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
        /// <summary>
        /// 获取懒加载的菜单树
        /// </summary>
        /// <param name="ParentMenuID">父节点ID</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMenu(decimal? ParentMenuID,int page,int limit)
            => Ok(menuService.GetLazyMenuTreeNode(ParentMenuID,page,limit));
        /// <summary>
        /// 插入菜单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateMenu([FromBody] menuinfo entity)
            => Ok(menuService.Insert(entity));
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateMenu([FromBody] menuinfo entity)
            => Ok(menuService.UpdateIgnoreColumns(entity,t=>new { t.ID},t=>new { t.MenuParentID,t.CreateBy}));
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("{ID}")]
        public IActionResult DeleteMenu(decimal ID)
            => Ok(menuService.Delete(t => t.ID == ID));

    }
}
