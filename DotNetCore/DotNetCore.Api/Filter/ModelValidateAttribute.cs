using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Api.Filter
{
    public class ModelValidateAttribute: Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        //
        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var s = context.ModelState;
        }
    }
}
