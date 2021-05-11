using System;
using System.Linq;
using System.Text;

namespace SqlSugarAndEntity
{
    ///<summary>
    ///
    ///</summary>
    public partial class menuinfo
    {
           public menuinfo(){


           }
           /// <summary>
           /// Desc:菜单ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:菜单名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string MenuName {get;set;}

           /// <summary>
           /// Desc:上级菜单ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? MenuParentID {get;set;}

           /// <summary>
           /// Desc:是否启用 0是1否
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int IsUse {get;set;}

           /// <summary>
           /// Desc:菜单路由
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string MenuRoute {get;set;}

           /// <summary>
           /// Desc:菜单的相对物理路径
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string MenuPath {get;set;}

           /// <summary>
           /// Desc:菜单的图标
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string MenuIcon {get;set;}

           /// <summary>
           /// Desc:菜单排序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? MenuSortNo {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? MenuCreateTime {get;set;}

           /// <summary>
           /// Desc:创建人ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? MenuCreateBy {get;set;}

    }
}
