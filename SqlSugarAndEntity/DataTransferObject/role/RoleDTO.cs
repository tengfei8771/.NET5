using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
