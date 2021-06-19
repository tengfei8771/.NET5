using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ApiFilter
{
    /// <summary>
    /// 包装结果忽略特性,标记此特性，不对返回值做包装
    /// </summary>
    public class ApiIgnoreAttribute:Attribute
    {
    }
}
