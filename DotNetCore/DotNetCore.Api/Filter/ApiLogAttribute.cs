using DotNetCore.Infrastruct.Extensions;
using DotNetCore.Infrastruct.Log;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Api.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiLogAttribute : ActionFilterAttribute
    {
        readonly ILogWriter logWriter = Log4NetWriter.GetInstance();
        private ApiInfoLogMsg msg;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            msg = new ApiInfoLogMsg();
            msg.RequestJson = context.ActionArguments.ToJson();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var action = (ControllerActionDescriptor)context.ActionDescriptor;
            msg.ControllerName = action.ControllerName;
            msg.ActionName = action.ActionName;
            msg.HttpMethod = context.HttpContext.Request.Method;
            msg.Path = context.HttpContext.Request.Path;
            msg.User = context.HttpContext.User.Identity.Name;
            msg.ResponseJson = context.Result is ObjectResult ? ((ObjectResult)context.Result).Value?.ToJson() : null;
            logWriter.ApiInfoLog(msg);
        }
    }
}
