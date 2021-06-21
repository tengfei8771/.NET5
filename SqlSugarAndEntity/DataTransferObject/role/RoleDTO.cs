using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.JsonConvertHelper;

namespace SqlSugarAndEntity.DataTransferObject.role
{
    public class RoleDTO
    {
        public class RoleForMenu
        {
            
            public decimal RoleID { get; set; }
            
            public List<decimal> MenuID { get; set; }
        }
        public class RoleForUser
        {
            
            public decimal RoleID { get; set; }
            
            public List<decimal> UserID { get; set; }
        }

       
    }
}
