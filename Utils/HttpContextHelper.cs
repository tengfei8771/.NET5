using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class HttpContextHelper
    {
        /// <summary>
        /// 获取路由终结点方法
        /// </summary>
        /// <param name="context">http上下文</param>
        /// <returns></returns>
        public static Endpoint GetEndPoint(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Features.Get<IEndpointFeature>()?.Endpoint;
        }

        /// <summary>
        /// 获取自定义特性类方法
        /// </summary>
        /// <typeparam name="T">自定义控制器特性类</typeparam>
        /// <param name="context">数据库上下文</param>
        /// <returns></returns>
        public static T GetMetadata<T>(HttpContext context) where T:class
        {
            Endpoint EndPoint = GetEndPoint(context);
            if (EndPoint == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return EndPoint.Metadata.GetMetadata<T>();
        }
        /// <summary>
        /// 获取http的返回值并返回一个Jobject的弱类型的实体
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        public static async Task<JObject> GetResponse(HttpResponse httpResponse)
        {
            httpResponse.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(httpResponse.Body).ReadToEndAsync();
            httpResponse.Body.Seek(0, SeekOrigin.Begin);
            return JObject.Parse(text);
        }

    }
}
