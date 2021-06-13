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

        public List<orginfo> GetLazyOrgTree(decimal? ParentId,int page = 1, int limit = 1000, int total = 0)
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
                    CreateBy = t.CreateBy,
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
                    CreateBy = t.CreateBy,
                    hasChildren = SqlFunc.Subqueryable<orginfo>().Where(a => a.ParentOrgID == t.ID).Any(),
                })
                .ToDataTable();
            return OrgList;
        }
        public override List<orginfo> GetInfo(Expression<Func<orginfo, bool>> predicate)
        {
            return baseMethod.Db().Queryable<orginfo>().WhereIF(predicate != null, predicate).WithCache().ToList();
        }
        public override bool Insert(orginfo entity)
        {
            return baseMethod.Db().Insertable(entity).RemoveDataCache().ExecuteCommand() > 0;
        }

        public override void Insert(List<orginfo> list)
        {
            baseMethod.Db().Insertable(list).RemoveDataCache().ExecuteCommand();
        }
        public override bool Delete(Expression<Func<orginfo, bool>> WhereExp)
        {
            return baseMethod.Db().Deleteable<orginfo>().Where(WhereExp).RemoveDataCache().ExecuteCommand() > 0;
        }
        public override void Delete(List<orginfo> list)
        {
            baseMethod.Db().Deleteable(list).RemoveDataCache().ExecuteCommand();
        }
        public override bool Update(orginfo entity)
        {
            return baseMethod.Db().Updateable(entity).RemoveDataCache().ExecuteCommand() > 0;
        }
        public override bool Update(orginfo entity, Expression<Func<orginfo, object>> WhereSelect)
        {
            return baseMethod.Db().Updateable(entity).WhereColumns(WhereSelect).RemoveDataCache().ExecuteCommand() > 0;
        }
        public override bool Update(Expression<Func<orginfo, orginfo>> SetColumns, Expression<Func<orginfo, object>> WhereSelect)
        {
            return baseMethod.Db().Updateable(SetColumns).WhereColumns(WhereSelect).RemoveDataCache().ExecuteCommand() > 0;
        }
        public override bool UpdateIgnoreColumns(orginfo entity, Expression<Func<orginfo, object>> WhereSelect, Expression<Func<orginfo, object>> IgnoreExpress)
        {
            return baseMethod.Db().Updateable(entity).IgnoreColumns(IgnoreExpress).WhereColumns(WhereSelect).RemoveDataCache().ExecuteCommand() > 0;
        }
        public override bool UpdateAppiontColumns(orginfo entity, Expression<Func<orginfo, object>> WhereSelect, Expression<Func<orginfo, object>> UpdateExpress)
        {
            return baseMethod.Db().Updateable(entity).UpdateColumns(UpdateExpress).WhereColumns(WhereSelect).RemoveDataCache().ExecuteCommand() > 0;
        }

        public override bool Update(List<orginfo> list)
        {
            return baseMethod.Db().Updateable(list).RemoveDataCache().ExecuteCommand() > 0;
        }
        public override bool Update(List<orginfo> list, Expression<Func<orginfo, object>> WhereExp)
        {
            return baseMethod.Db().Updateable(list).WhereColumns(WhereExp).RemoveDataCache().ExecuteCommand() > 0;
        }
    }
}
