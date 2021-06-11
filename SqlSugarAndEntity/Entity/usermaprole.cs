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
        [JsonConverter(typeof(ConvertLongToString))]
        public decimal ID { get; set; }

        /// <summary>
        /// Desc:用户ID
        /// Default:
        /// Nullable:False
        /// </summary>     
        [JsonConverter(typeof(ConvertLongToString))]
        public decimal UserID { get; set; }

        /// <summary>
        /// Desc:权限ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        [JsonConverter(typeof(ConvertLongToString))]
        public decimal? RoleID { get; set; }

    }
}