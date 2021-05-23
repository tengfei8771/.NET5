using IRepository;
using SqlSugar;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OrgRepository:BaseRepository<orginfo>, IOrgRepository
    {
        private IBaseMethod baseMethod;
        public OrgRepository(IBaseMethod baseMethod) :base(baseMethod)
        {
            this.baseMethod = baseMethod;
        }

        public List<orginfo> GetLazyOrgTree(int? ParentId,int page = 1, int limit = 1000, int total = 0)
        {
            Expression<Func<orginfo, bool>> exp;
            if (ParentId == null)
            {
                exp = t => t.ParentOrgID == null;
            }
            else
            {
                exp = t => t.ParentOrgID == ParentId;
            }
            var OrgList = baseMethod.Db().Queryable<orginfo>()
                .Where(exp)
                .Select(t => new orginfo
                {
                    ID = t.ID,
                    OrgName = t.OrgName,
                    OrgCode = t.OrgCode,
                    ParentOrgID = t.ParentOrgID,
                    ShortName = t.ShortName,
                    Remark = t.Remark,
                    OrgCreateTime = t.OrgCreateTime,
                    OrgCreateBy = t.OrgCreateBy,
                    hasChildren = SqlFunc.Subqueryable<orginfo>().Where(a => a.ParentOrgID == t.ID).Any(),
                })
                .Mapper(it =>
                {
                    if (it.hasChildren)
                    {
                        it.children = new List<orginfo>();
                    }
                })
                .ToPageList(page, limit, ref total);
            return OrgList;
                
        }

        public DataTable GetOrgTree()
        {
            var OrgList = baseMethod.Db().Queryable<orginfo>()
                .Where(t => t.ParentOrgID == null)
                .Select(t => new orginfo
                {
                    ID = t.ID,
                    OrgName = t.OrgName,
                    OrgCode = t.OrgCode,
                    ParentOrgID = t.ParentOrgID,
                    ShortName = t.ShortName,
                    Remark = t.Remark,
                    OrgCreateTime = t.OrgCreateTime,
                    OrgCreateBy = t.OrgCreateBy,
                    hasChildren = SqlFunc.Subqueryable<orginfo>().Where(a => a.ParentOrgID == t.ID).Any(),
                })
                .ToDataTable();
            return OrgList;
        }
    }
}
