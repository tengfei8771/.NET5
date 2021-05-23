using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarAndEntity.DataTransferObject.menu
{
    public class MenuDTO
    {
        public decimal RoleID { get; set; }
        public List<string> menuID { get; set; }
    }
}
