using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SqlSugarAndEntity.DataTransferObject.role
{
    public class RoleDTO
    {
        public class RoleForMenu
        {
            [JsonConverter(typeof(ConvertSturtToString))]
            public decimal RoleID { get; set; }
            [JsonConverter(typeof(ConvertSturtToString))]
            public List<decimal> MenuID { get; set; }
        }
        public class RoleForUser
        {
            [JsonConverter(typeof(ConvertSturtToString))]
            public decimal RoleID { get; set; }
            [JsonConverter(typeof(ConvertSturtToString))]
            public List<decimal> UserID { get; set; }
        }

       
    }
}
