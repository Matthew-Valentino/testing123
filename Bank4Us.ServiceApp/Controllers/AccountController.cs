using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bank4Us.Common.CanonicalSchema;
using Bank4Us.BusinessLayer.Managers.AccountManagement;
using System.Net.Http;
using System.Net;
using Bank4Us.ServiceApp.Filters;
using Bank4Us.BusinessLayer.Core;
using Microsoft.Extensions.Logging;
using Bank4Us.Common.Facade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Bank4Us.Common.Core;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bank4Us.ServiceApp.Controllers
{

    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2019
    ///   Name: William J Leannah
    ///   Description: Example implementation of a Service App with MVC           
    /// </summary>
    /// 

    // [Authorize]
    [LoggingActionFilter]
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IAccountManager _manager;
        private ILogger _logger;

        public AccountController(SignInManager<ApplicationUser> signInManager, IAccountManager manager, ILogger<AccountController> logger) : base(manager, logger)
        {
            _manager = manager;
            _logger = logger;
            _signInManager = signInManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToPage("/Index");
        }

        // GET: api/values

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
        [Route("accounts")]
        public IActionResult GetAllAccounts()
        {
            try
            {
                var items = _manager.GetAllAccounts();
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
        public IActionResult Post(Account account)
        {
            try
            {
                _manager.Create(account);
                return new OkResult();

            }catch(Exception ex)
            {
                _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
                return new EmptyResult();
            }
        }
        [TransactionActionFilter()]
        [HttpPut]
        public IActionResult Put(Account account)
        {
            try
            {
                _manager.Update(account);
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
        public IActionResult Delete(Account account)
        {
            try
            {
                _manager.Delete(account);
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
        [Route("accounts/{accountId}")]
        public IActionResult GetAccountByAccountId(int accountId)
        {
           try 
            {
                var account = _manager.GetAccount(accountId);
                if (account != null)
                {
                    return new OkObjectResult(account);
                }
                else
                {
                    return NotFound();
                }
            }catch(Exception ex)
            {
                _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
                return new EmptyResult();
            }
        }
    }
        
}
