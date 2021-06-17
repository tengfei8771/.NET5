using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IServices.ResModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utils;
using WebApi.Common;
using static Utils.JwtHelper;

namespace PublicWebApi.Common.Validator
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        { 
            if (Verification.GetFlag(httpContext))
            {
                string key = GetRequestKey();
                ResponseModel responseModel = new ResponseModel();
                HttpRequest request = httpContext.Request;
                #region 拦截需要验证的请求
                if (!request.Headers.TryGetValue("Bear", out var apiKeyHeaderValues))
                {
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    responseModel.code = (int)ResponseTypeEnum.NoToken;
                    responseModel.message = ReflectionConvertHelper.GetEnumDescription(ResponseTypeEnum.NoToken);
                    httpContext.Response.WriteAsync(JsonConvert.SerializeObject(responseModel));
                    return Task.FromResult(0);
                }
                else
                {
                    request.EnableBuffering();//可以多次多次读取http内包含的数据
                    var JwtValidateData = ValidateJwt(key,apiKeyHeaderValues.ToString());
                    if (!JwtValidateData.Item1)
                    {
                        httpContext.Response.ContentType = "application/json";
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        responseModel.code =(int)JwtValidateData.Item2;
                        responseModel.message = ReflectionConvertHelper.GetEnumDescription(JwtValidateData.Item2);
                        httpContext.Response.WriteAsync(JsonConvert.SerializeObject(responseModel));
                        return Task.FromResult(0);
                        //return;
                    }
                }
                #endregion
                
            }
            return  _next(httpContext);
            #region 拦截返回结果,记录日志
            //var originalBodyStream = httpContext.Response.Body;
            //JObject obj = new JObject();
            //using (var responseBody = new MemoryStream())
            //{
            //    httpContext.Response.Body = responseBody;
            //    await _next(httpContext);
            //    obj = await HttpContextHelper.GetResponse(httpContext.Response);
            //    await responseBody.CopyToAsync(originalBodyStream);
            //    string res = JsonConvert.SerializeObject(obj);
            //    httpContext.Response.Body = originalBodyStream;
            //    httpContext.Response.ContentLength = res.Length;
            //    await httpContext.Response.WriteAsync(res);
            //}
            //await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(obj));

            #endregion
            //httpContext.Response.OnCompleted(() =>
            //{
            //    return Task.CompletedTask;
            //});
        }

    }
    

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
