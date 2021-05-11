using System;
using System.Linq;
using System.Text;

namespace SqlSugarAndEntity
{
    ///<summary>
    ///
    ///</summary>
    public partial class usermaporg
    {
           public usermaporg(){


           }
           /// <summary>
           /// Desc:主键
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
           /// Desc:组织机构ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal OrgID {get;set;}

    }
}
