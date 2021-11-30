using Bank4Us.Common.Facade;
using Bank4Us.ServiceApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank4Us.ServiceApp.Filters
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2021
    ///   Name: William J Leannah
    ///   Description: Example implementation of a Service App with MVC           
    /// </summary>
    /// 

    public class LoggingActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            Log("OnActionExecuting", context.RouteData, context.Controller);
          
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Log("OnActionExecuted", context.RouteData, context.Controller);

        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Log("OnResultExecuted", context.RouteData, context.Controller);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Log("OnResultExecuting", context.RouteData, context.Controller);
        }



        private void Log(string methodName, RouteData routeData, Object controller)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
            BaseController baseController = ((BaseController)controller);
            baseController.Logger.LogInformation(LoggingEvents.ACCESS_METHOD, message);
        }
    }
}
