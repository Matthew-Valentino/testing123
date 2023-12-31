﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Bank4Us.Common.Core;
using Newtonsoft.Json;

namespace Bank4Us.Common.CanonicalSchema
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2023
    ///   Name: William J Leannah
    ///   Description: Example implementation of Entity Framework Core.
    ///                 http://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx  
    /// </summary>
    /// 
    public class Customer : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public string FirstName { get; set; }
        [MaxLength(255)]
        public string LastName { get; set; }
        [MaxLength(255)]
        public string EmailAddress { get; set; }
        public List<Account> Accounts { get; set; }
        public int Age { get; set; }
    }
}
