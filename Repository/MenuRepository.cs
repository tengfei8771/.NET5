using IRepository;
using SqlSugar;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MenuRepository:BaseRepository<menuinfo>, IMenuRepository
    {
        private IBaseMethod baseMethod;
        public MenuRepository(IBaseMethod baseMethod):base(baseMethod)
        {
            this.baseMethod = baseMethod;
        }

        public List<menuinfo> GetLazyMenuTreeNode(decimal? ParentMenuID)
        {
            Expression<Func<menuinfo, bool>> exp;
            if (ParentMenuID != null)
            {
                exp = t => t.MenuParentID == ParentMenuID;
            }
            else
            {
                exp = t => t.MenuParentID == null;
            }
            var list = baseMethod.Db().Queryable<menuinfo>()
                .Where(exp)
                .Select(t => new menuinfo
                {
                    ID = t.ID,
                    MenuName = t.MenuName,
                    MenuParentID = t.MenuParentID,
                    IsUse = t.IsUse,
                    MenuRoute = t.MenuRoute,
                    MenuPath = t.MenuPath,
                    MenuIcon = t.MenuIcon,
                    MenuSortNo = t.MenuSortNo,
                    MenuCreateTime = t.MenuCreateTime,
                    MenuCreateBy = t.MenuCreateBy,
                    hasChildren = SqlFunc.Subqueryable<menuinfo>().Where(a => a.MenuParentID == t.ID).Any()
                })
                .Mapper(it =>
                {
                    if (it.hasChildren)
                    {
                        it.children = new List<menuinfo>();
                    }
                })
                .ToList();
            return list;     
        }

        public List<menuinfo> GetMenubyRole(decimal userId)
        {
            var list = baseMethod.Db().Queryable<userinfo, usermaprole, rolemapmenu, menuinfo>
                ((a, b, c, d) => new JoinQueryInfos(
                    JoinType.Inner, a.ID == b.UserID,
                    JoinType.Inner, b.RoleID == c.RoleID,
                    JoinType.Inner, c.MenuID == d.ID
                    ))
                .Distinct()
                .Where((a,b,c,d)=>a.ID==userId)
                .Select((a, b, c, d) => d)
                .ToList();
            return list;
        }
    }
}
