﻿using IRepository;
using IServices;
using IServices.ResModel;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Services
{
    public class OrgService:BaseService<orginfo>, IOrgService
    {
        private IOrgRepository orgRepository;
        public OrgService(IOrgRepository orgRepository) :base(orgRepository)
        {
            this.orgRepository = orgRepository;
        }

        public ResponseModel GetLazyOrgTree(decimal? ParentId, int page = 1, int limit = 1000)
        {
            int total = 0;
            return CreateResponseModel(orgRepository.GetLazyOrgTree, ParentId, page, limit, total);
        }


        public ResponseModel GetOrgTree()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                DataTable dt = orgRepository.GetOrgTree();
                var list = ReflectionConvertHelper.ConvertDatatableToTreeList<orginfo>(dt, "ID", "ParentID");
                res.code = 2000;
                res.items = list;
                res.message = "成功";
                res.total = list.Count;
            }
            catch(Exception e)
            {
                res.code = -1;
                res.message = e.Message;
            }
            return res;  
        }
        public List<decimal> GetOrgChildrenByID(decimal ParentId)
        {
            //获取全部的组织树
            var totalList = orgRepository.GetInfo(t => true);
            List<decimal> IdList = new List<decimal>();
            IdList.Add(ParentId);
            GetChildrenCode(totalList, ParentId, IdList);
            return IdList;
        }
        private void GetChildrenCode(List<orginfo> source,decimal ParentId,List<decimal> IdList)
        {
            foreach(var item in source.Where(t => t.ParentOrgID == ParentId))
            {
                IdList.Add(item.ID);
                GetChildrenCode(source, item.ID, IdList);
            }
        }
    }
}
