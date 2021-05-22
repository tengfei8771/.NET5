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
        ResponseModel GetLazyOrgTree(int? ParentId, int page = 1, int limit = 1000);
        ResponseModel GetOrgTree();
    }
}
