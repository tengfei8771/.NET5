using IServices.ResModel;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IOrgService:IBaseService<orginfo>
    {
        /// <summary>
        /// 根据parentid获取懒加载的下一级节点数据
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        ResponseModel GetLazyOrgTree(decimal? ParentId, int page = 1, int limit = 1000);
        /// <summary>
        /// 获取一个完整的组织机构树
        /// </summary>
        /// <returns></returns>
        ResponseModel GetOrgTree();
        /// <summary>
        /// 根据传入的父节点ID获取全部的子节点ID
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        List<decimal> GetOrgChildrenByID(decimal ParentId);
    }
}
