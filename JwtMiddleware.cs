using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IServices.ResModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

        public  Task Invoke(HttpContext httpContext)
        { 
            if (Verification.GetFlag(httpContext))
            {
                ResponseModel responseModel = new ResponseModel();
                HttpRequest request = httpContext.Request;
                if (!request.Headers.TryGetValue("Bear", out var apiKeyHeaderValues))
                {
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    responseModel.code = 50014;
                    responseModel.message = "此请求未包含请求令牌,禁止访问!";
                    httpContext.Response.WriteAsync(JsonConvert.SerializeObject(responseModel));
                    return Task.FromResult(0);
                }
                else
                {
                    request.EnableBuffering();//可以多次多次读取http内包含的数据
                    var JwtValidateData = ValidateJwt(apiKeyHeaderValues.ToString());
                    if (!JwtValidateData.Item1)
                    {
                        httpContext.Response.ContentType = "application/json";
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        if (JwtValidateData.Item2 == (int)ResponseType.IllegalToken)
                        {
                            responseModel.code = 50015;
                            responseModel.message = "非法令牌!,禁止访问！";
                        }
                        else
                        {
                            responseModel.code = 50016;
                            responseModel.message = "令牌已过期,请重新登录";
                        }
                        httpContext.Response.WriteAsync(JsonConvert.SerializeObject(responseModel));
                        return Task.FromResult(0);
                    }
                }
                // _next(httpContext);
                //var res = HttpContextHelper.GetHttpResponse(httpContext.Response);
            }
             return _next(httpContext);
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
