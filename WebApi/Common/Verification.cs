using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace WebApi.Common
{
    /// <summary>
    /// 验证类,判断控制器权限是否需要jwt验证
    /// </summary>
    public class Verification
    {
        /// <summary>
        /// 获取判断标志位
        /// </summary>
        /// <param name="context">http上下文</param>
        /// <returns></returns>
        public static bool GetFlag(HttpContext context)
        {
            bool flag = false;
            var Endpoint = HttpContextHelper.GetEndPoint(context);
            if (Endpoint == null)
            {
                return flag;
            }
            var attributes = Endpoint.Metadata;
            var  AllowAny= attributes.GetMetadata<AllowAnyJwtVerificationAttribute>();
            var Valid = attributes.GetMetadata<JwtVerificationAttribute>();
            var attributesList = attributes.ToList();
            int AllowIndex = AllowAny == null ? -1 : attributesList.IndexOf(AllowAny);
            int ValidIndex = Valid == null ? -1 : attributesList.IndexOf(Valid);
            if (ValidIndex > AllowIndex)
            {
                flag = true;
            }
            return flag;
        }
    }
}
