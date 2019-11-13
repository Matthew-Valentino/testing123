using Bank4Us.BusinessLayer.Core;
using Bank4Us.Common.CanonicalSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank4Us.BusinessLayer.Managers.CustomerManagement
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2019
    ///   Name: William J Leannah
    ///   Description: Example implementation of a Business Layer              
    /// </summary>
    /// 
    public interface ICustomerManager : IActionManager
    {
        Customer GetCustomer(int customerId);
        IEnumerable<Customer> GetAllCustomers();
    }
}
