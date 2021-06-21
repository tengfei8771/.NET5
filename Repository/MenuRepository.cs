using IBaseRepository;
using SqlSugar;
using SqlSugarAndEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static SqlSugarAndEntity.DataTransferObject.role.RoleDTO;

namespace Repository
{
    public class MenuRepository:BaseRepository<menuinfo>, IMenuRepository
    {
        private IBaseMethod baseMethod;
        public MenuRepository(IBaseMethod baseMethod):base(baseMethod)
        {
            this.baseMethod = baseMethod;
        }

        public List<menuinfo> GetLazyMenuTreeNode(decimal? ParentMenuID,int page,int limit,ref int total)
        {
            Expression<Func<menuinfo, bool>> exp;
            if (ParentMenuID != null)
            {
                exp = t => t.MenuParentID == ParentMenuID;
            }
            else
            {
                exp = t => t.MenuParentID == null|| t.MenuParentID==0;
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
                    CreateBy = t.CreateBy,
                    hasChildren = SqlFunc.Subqueryable<menuinfo>().Where(a => a.MenuParentID == t.ID).Any()
                })
                .OrderBy(t => new {t.MenuSortNo})
                .Mapper(it =>
                {
                    if (it.hasChildren)
                    {
                        it.children = new List<menuinfo>();
                    }
                })
                .ToPageList(page,limit,ref total);
            return list;     
        }

        public RoleForMenu GetMenuByRoleId(decimal roleId)
        {
            var list = baseMethod.Db().Queryable<rolemapmenu>().Where(t => t.RoleID == roleId)
                .Select(t => t.MenuID).ToList();
            RoleForMenu roleForMenu = new RoleForMenu()
            {
                RoleID = roleId,
                MenuID = list
            };
            return roleForMenu;
        }

        public List<menuinfo> GetMenuByUserId(decimal userId)
        {
            var list = baseMethod.Db().Queryable<userinfo, usermaprole, rolemapmenu, menuinfo>
                ((a, b, c, d) => new JoinQueryInfos(
                    JoinType.Inner, a.ID == b.UserID,
                    JoinType.Inner, b.RoleID == c.RoleID,
                    JoinType.Inner, c.MenuID == d.ID
                    ))
                .Distinct()
                .Where((a,b,c,d)=>a.ID==userId)
                .OrderBy((a, b, c, d) => new {d.MenuSortNo})
                .Select((a, b, c, d) => d)
                .ToList();
            return list;
        }

        public DataTable GetMenuTree()
        {
            DataTable dt = baseMethod.Db().Queryable<menuinfo>().ToDataTable();
            return dt;
        }
    }
}
