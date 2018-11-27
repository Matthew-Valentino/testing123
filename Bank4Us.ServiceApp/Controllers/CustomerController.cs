using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bank4Us.ServiceApp.Filters;
using Bank4Us.Common.CanonicalSchema;
using Bank4Us.BusinessLayer.Managers.CustomerManagement;
using Bank4Us.Common.Facade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bank4Us.ServiceApp.Controllers
{

    /// <summary>
    ///   Course Name: MSCS 6360 Enterprise Architecture
    ///   Year: Fall 2018
    ///   Name: William J Leannah
    ///   Description: Example implementation of a Service App with MVC           
    /// </summary>
    /// 
    [LoggingActionFilter]
    [Route("api/[controller]")]
    public class CustomerController : BaseController
    {
         
        ICustomerManager _manager;
        ILogger<CustomerController> _logger;

        public CustomerController(ICustomerManager manager, ILogger<CustomerController> logger) : base(manager, logger)
        {
            _manager = manager;
            _logger = logger;
        }

        [TransactionActionFilter()]
        [HttpGet]
        [Route("baseentities")]
        public IActionResult GetAllBaseEntities()
        {
            try
            {
                var items = _manager.GetAll();
                return new OkObjectResult(items);

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
                return new EmptyResult();
            }
        }

        [TransactionActionFilter()]
        [HttpGet]
        [Route("customers")]
        [Authorize]
        public IActionResult GetAllCustomers()
        {
            try
            {
                var items = _manager.GetAllCustomers();
                return new OkObjectResult(items);

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
                return new EmptyResult();
            }
        }


        [TransactionActionFilter()]
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            try
            {
                _manager.Create(customer);
                return new OkResult();

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
                return new EmptyResult();
            }
        }
        [TransactionActionFilter()]
        [HttpPut]
        public IActionResult Put(Customer customer)
        {
            try
            {
                _manager.Update(customer);
                return new OkResult();

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
                return new EmptyResult();
            }
        }

        [TransactionActionFilter()]
        [HttpDelete]
        public IActionResult Delete(Customer customer)
        {
            try
            {
                _manager.Delete(customer);
                return new OkResult();

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
                return new EmptyResult();
            }
        }


        [TransactionActionFilter()]
        [HttpGet]
        [Route("customers/{customerId}")]
        public IActionResult GetCustomerByCustomerId(int customerId)
        {
            try
            {
                var account = _manager.GetCustomer(customerId);
                if (account != null)
                {
                    return new OkObjectResult(account);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
                return new EmptyResult();
            }
        }


    }
}

      