using System;
using System.Linq;
using System.Text;

using Utils;
namespace SqlSugarAndEntity
{
    ///<summary>
    ///
    ///</summary>
    public partial class usermaprole
    {
           public usermaprole()
           {
                this.ID = SnowflakeHelper.GetId();
                
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