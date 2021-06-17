using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.JsonConvertHelper;

namespace SqlSugarAndEntity.DataTransferObject.menu
{
    public class MenuDTO
    {
        [JsonConverter(typeof(ConvertSturtToString))]
        public decimal RoleID { get; set; }
        [JsonConverter(typeof(ConvertSturtToString))]
        public List<string> menuID { get; set; }
    }
}
