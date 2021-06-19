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
    public class ApiResultFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var isDefined = controllerActionDescriptor.EndpointMetadata.Any(a => a.GetType().Equals(typeof(ApiIgnoreAttribute)));
                if (isDefined)
                {
                    return;
                }
            }
            //对返回结果进行检查,如果没设置返回code和消息,那么默认置为成功
            if (context.Result != null)
            {
                if (context.Result is ObjectResult)
                {
                    var result = context.Result as ObjectResult;
                    //如果已经做好返回模型,那么就判断code是否存在就可以了
#pragma warning disable CS8602 // 解引用可能出现空引用。
                    if (result.Value.GetType() == typeof(ResponseModel))
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    {
                        var res = result.Value as ResponseModel;
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        if (res.code == Utils.ResponseTypeEnum.NotSetValue)
#pragma warning restore CS8602 // 解引用可能出现空引用。
                        {
                            //返回数据对象为空 代表操作不是查询
                            if (res.items == null)
                            {
                                res.code = Utils.ResponseTypeEnum.OperationSucess;
                                res.message = Utils.ResponseTypeEnum.OperationSucess;
                            }
                            else
                            {
                                res.code = Utils.ResponseTypeEnum.GetInfoSucess;
                                res.message = Utils.ResponseTypeEnum.GetInfoSucess;
                            }
                        }
                        context.Result = new JsonResult(res);
                    }
                    else
                    {
                        context.Result = new JsonResult(new ResponseModel() 
                        { 
                            code= Utils.ResponseTypeEnum.Success,
                            message = Utils.ResponseTypeEnum.Success
                        });
                    }
                   
                    

                }
                else if (context.Result is EmptyResult)
                {
                    context.Result = context.Result = new JsonResult(new ResponseModel()
                    {
                        code = Utils.ResponseTypeEnum.Success,
                        message = Utils.ResponseTypeEnum.Success
                    });
                }
                else if (context.Result is ContentResult)
                {
                    var result = context.Result as ContentResult;
                    context.Result = context.Result = new JsonResult(new ResponseModel()
                    {
                        code = Utils.ResponseTypeEnum.Success,
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        message = result.Content
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    });
                }
                else
                {
                    throw new Exception($"未经处理的Result类型：{ context.Result.GetType().Name}");
                }

            }

        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
