using System;
using System.Collections.Generic;
using System.Text;

namespace Bank4Us.CanonicalSchema
{
    /// <summary>
    ///   Course Name: MSCS 6360 Enterprise Architecture
    ///   Year: Fall 2018
    ///   Name: William J Leannah
    ///   Description: Example implementation of the Domain Driven Design Pattern.
    ///                https://en.wikipedia.org/wiki/Domain-driven_design                 
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
