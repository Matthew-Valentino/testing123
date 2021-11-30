using Bank4Us.BusinessLayer.Core;
using Bank4Us.ServiceApp.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
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
    public class TransactionActionFilter : ActionFilterAttribute
    {
        IDbContextTransaction transaction;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ((BaseController)context.Controller).ActionManager.UnitOfWork.BeginTransaction();
            
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            
            if (context.Exception != null)
            {
                ((BaseController)context.Controller).ActionManager.UnitOfWork.RollbackTransaction();
            }
            else
            {
                ((BaseController)context.Controller).ActionManager.UnitOfWork.CommitTransaction();
            }
        }

    }
}
