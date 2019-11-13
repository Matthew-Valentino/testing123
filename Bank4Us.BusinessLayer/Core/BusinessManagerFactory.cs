using Bank4Us.BusinessLayer.Managers.CustomerManagement;
using Bank4Us.BusinessLayer.Managers.AccountManagement;
using Bank4Us.DataAccess.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank4Us.BusinessLayer.Core
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2019
    ///   Name: William J Leannah
    ///   Description: Example implementation of a Business Layer              
    /// </summary>
    /// 
    public class BusinessManagerFactory  
    {
        ICustomerManager _customerManager;
        IAccountManager _accountManager;
        public BusinessManagerFactory(ICustomerManager customerManager=null, IAccountManager accountManager=null)
        {
            _customerManager = customerManager;
            _accountManager = accountManager;
        }

        public ICustomerManager GetCustomerManager()
        {
            return _customerManager;
        }

        public IAccountManager GetAccountManager()
        {
            return _accountManager;
        }

    }

   

}
