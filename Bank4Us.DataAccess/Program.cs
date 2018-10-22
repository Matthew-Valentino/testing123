using System;
using System.Collections.Generic;
using Bank4Us.Common.CanonicalSchema;
using Bank4Us.DataAccess.Core;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Bank4Us.DataAccess
{
    /// <summary>
    ///   Course Name: MSCS 6360 Enterprise Architecture
    ///   Year: Fall 2018
    ///   Name: William J Leannah
    ///   Description: Example implementation of Entity Framework Core.
    ///                 http://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx  
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //INFO: Create a new database context.  
            DataContext db = new DataContext();

            //INFO: Create a new customer.  
            Customer c = new Customer()
            {
                FirstName = "Fred",
                LastName = "Jones",
                EmailAddress = "Fred.Jones@gmail.com",
                CreatedBy = "admin",
                CreatedOn = DateTime.Now
            };

            //INFO: Create a new account.  
            Account a = new Account()
            {
                Type = "Checking",
                OpenDate = DateTime.Now,
                Balance = 500.00m
            };

            //INFO: Link the account to the customer.
            c.Accounts = new List<Account>();
            c.Accounts.Add(a);
            //INFO: Add the customer to the database.
            db.Customers.Add(c);
            //INFO: Save the changes.  
            db.Save();
        }
    }
}
