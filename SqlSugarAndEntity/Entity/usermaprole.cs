using System;
using System.Linq;
using System.Text;

namespace SqlSugarAndEntity
{
    ///<summary>
    ///
    ///</summary>
    public partial class usermaprole
    {
           public usermaprole(){


           }
           /// <summary>
           /// Desc:ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:用户ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal UserID {get;set;}

           /// <summary>
           /// Desc:权限ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? RoleID {get;set;}

    }
}
