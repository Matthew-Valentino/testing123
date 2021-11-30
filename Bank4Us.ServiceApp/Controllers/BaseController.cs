
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bank4Us.BusinessLayer.Core;
using Microsoft.Extensions.Logging;
using Bank4Us.Common.Facade;
using System.Web.Http;
using System.Net.Http;
using System.Text;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace  Bank4Us.ServiceApp.Controllers
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2021
    ///   Name: William J Leannah
    ///   Description: Example implementation of a Service App with MVC           
    /// </summary>
    /// 
    public class BaseController : Controller
    {
        private IActionManager _manager;
        private ILogger _logger;
        public BaseController(IActionManager manager, ILogger logger)
        {
            _manager = manager;
            _logger = logger;
        }
        public IActionManager ActionManager { get { return _manager; } }
        public ILogger Logger { get { return _logger; } }
       
        protected HttpResponseException LogException(Exception ex)
        {
            string errorMessage = LoggerHelper.GetExceptionDetails(ex);
            _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, errorMessage);
            HttpResponseMessage message = new HttpResponseMessage();
            message.Content = new StringContent(errorMessage);
            message.StatusCode = System.Net.HttpStatusCode.ExpectationFailed;
            throw new HttpResponseException(message);
        }

    }
}
