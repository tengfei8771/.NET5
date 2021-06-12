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
    public partial class usermaporg
    {
        public usermaporg()
        {
            this.ID = SnowflakeHelper.GetId();

        }
        /// <summary>
        /// Desc:主键
        /// Default:
        /// Nullable:False
        /// </summary>           
        [JsonConverter(typeof(ConvertSturtToString))]
        public decimal ID {get;set;}

        /// <summary>
        /// Desc:用户ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        [JsonConverter(typeof(ConvertSturtToString))]
        public decimal UserID {get;set;}

        /// <summary>
        /// Desc:组织机构ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        [JsonConverter(typeof(string))]
        public decimal OrgID {get;set;}

    }
}