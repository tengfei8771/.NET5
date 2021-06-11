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
    public partial class orginfo
    {
        public orginfo()
        {
            this.ID = SnowflakeHelper.GetId();

        }
        /// <summary>
        /// Desc:组织机构ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        [JsonConverter(typeof(ConvertLongToString))]
        public decimal ID { get; set; }

        /// <summary>
        /// Desc:组织机构名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string OrgName { get; set; }

        /// <summary>
        /// Desc:组织机构编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string OrgCode { get; set; }

        /// <summary>
        /// Desc:上级组织机构编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? ParentOrgID { get; set; }

        /// <summary>
        /// Desc:组织机构简称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ShortName { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Remark { get; set; }

        /// <summary>
        /// Desc:创建人ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? CreateBy { get; set; }

        [SugarColumn(IsIgnore = true)]
        public bool hasChildren { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<orginfo> children { get; set; }
    }
}