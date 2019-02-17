using DotNetCore.Infrastruct.Log;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Api.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        readonly ILogWriter logWriter = Log4NetWriter.GetInstance();

        public override void OnException(ExceptionContext context)
        {
            var action = (ControllerActionDescriptor)context.ActionDescriptor;
            ApiErrorLogMsg msg = new ApiErrorLogMsg
            {
                 HttpMethod=context.HttpContext.Request.Method,
                  ControllerName=action.ControllerName,
                  ActionName=action.ActionName,
                   Path=context.HttpContext.Request.Path,
                    User=context.HttpContext.User.Identity.Name
                    //RequestJson=context.ActionDescriptor.
            };
        }
    }
}
