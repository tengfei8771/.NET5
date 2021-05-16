using System;
using System.Linq;
using System.Text;

using Utils;
namespace SqlSugarAndEntity
{
    ///<summary>
    ///
    ///</summary>
    public partial class userinfo
    {
           public userinfo()
           {
                this.ID = SnowflakeHelper.GetId();
                
           }
           /// <summary>
           /// Desc:主键
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:用户姓名
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string UserName {get;set;}

           /// <summary>
           /// Desc:登录账号
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string UserAccount {get;set;}

           /// <summary>
           /// Desc:密码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string UserPassWord {get;set;}

           /// <summary>
           /// Desc:性别
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string UserSex {get;set;}

           /// <summary>
           /// Desc:用户手机
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string UserPhone {get;set;}

           /// <summary>
           /// Desc:用户权限 0管理员 1普通用户
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string UserRole {get;set;}

           /// <summary>
           /// Desc:身份证号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string IdNumber {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? UserCreateTime {get;set;}

           /// <summary>
           /// Desc:创建人ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? UserCreateBy {get;set;}

    }
}