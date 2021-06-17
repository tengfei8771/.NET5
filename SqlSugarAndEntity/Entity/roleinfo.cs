using System;
using System.Linq;
using System.Text;

using Utils;
using System.Collections.Generic;
using SqlSugar;
using Newtonsoft.Json;
using Utils.JsonConvertHelper;

namespace SqlSugarAndEntity
{
    ///<summary>
    ///
    ///</summary>
    public partial class roleinfo
    {
           public roleinfo()
           {
                this.ID = SnowflakeHelper.GetId();
                
           }
        /// <summary>
        /// Desc:角色ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        [JsonConverter(typeof(ConvertSturtToString))]
        public decimal ID {get;set;}

           /// <summary>
           /// Desc:角色名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string RoleName {get;set;}

           /// <summary>
           /// Desc:创建人ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? CreateBy {get;set;}

    }
}