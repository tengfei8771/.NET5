﻿using System;
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
    public partial class dictionary
    {
        public dictionary()
        {
            this.ID = IDHelper.GetId();

        }
        /// <summary>
        /// Desc:主键
        /// Default:
        /// Nullable:False
        /// </summary>           
        
        public decimal ID { get; set; }

        /// <summary>
        /// Desc:名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:父节点的ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        
        public decimal? ParentID { get; set; }

        /// <summary>
        /// Desc:排列顺序
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? SortNO { get; set; }

        [SugarColumn(IsIgnore = true)]
        public bool hasChildren { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<dictionary> children { get; set; }
    }
}