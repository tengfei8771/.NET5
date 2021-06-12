using System;
using System.Linq;
using System.Text;

using Utils;
using System.Collections.Generic;
using SqlSugar;
using Newtonsoft.Json;

namespace SqlSugarAndEntity
{
    ///<summary>
    ///
    ///</summary>
    public partial class rolemapmenu
    {
           public rolemapmenu()
           {
                this.ID = SnowflakeHelper.GetId();
                
           }
        /// <summary>
        /// Desc:ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        [JsonConverter(typeof(ConvertSturtToString))]
        public decimal ID {get;set;}

        /// <summary>
        /// Desc:角色ID
        /// Default:
        /// Nullable:False
        /// </summary>     
        [JsonConverter(typeof(ConvertSturtToString))]
        public decimal RoleID {get;set;}

        /// <summary>
        /// Desc:菜单ID
        /// Default:
        /// Nullable:False
        /// </summary>       
        [JsonConverter(typeof(ConvertSturtToString))]
        public decimal MenuID {get;set;}

    }
}