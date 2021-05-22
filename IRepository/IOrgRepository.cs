using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IOrgRepository:IBaseRepository<orginfo>
    {
        List<orginfo> GetLazyOrgTree(int? ParentId,int page=1,int limit=1000,int total=0);

        DataTable GetOrgTree();

    }
}
