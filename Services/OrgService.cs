using IRepository;
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

        public ResponseModel GetLazyOrgTree(int? ParentId, int page = 1, int limit = 1000)
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
                res.Code = 2000;
                res.Items = list;
                res.Message = "成功";
                res.Total = list.Count;
            }
            catch(Exception e)
            {
                res.Code = -1;
                res.Message = e.Message;
            }
            return res;  
        }
    }
}
