using IServices.ResModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ApiFilter
{
    public class ApiExceptionAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //处理过异常就不再进行处理
            if (context.ExceptionHandled) return;
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var isDefined = controllerActionDescriptor.EndpointMetadata.Any(a => a.GetType().Equals(typeof(ApiIgnoreAttribute)));
                if (isDefined)
                {
                    return;
                }
            }
            //异常处理标志位置为true
            context.ExceptionHandled = true;
            context.Result = new JsonResult(new ResponseModel()
            {
                code = Utils.ResponseTypeEnum.Exception,
                message = context.Exception.Message
            });
        }
    }
}
