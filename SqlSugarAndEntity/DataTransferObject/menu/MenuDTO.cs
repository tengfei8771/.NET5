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
        
        public decimal RoleID { get; set; }
        
        public List<string> menuID { get; set; }
    }
}
