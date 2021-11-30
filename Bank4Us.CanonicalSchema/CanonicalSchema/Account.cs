using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Bank4Us.Common.Core;
using Newtonsoft.Json;

namespace Bank4Us.Common.CanonicalSchema
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2021
    ///   Name: William J Leannah
    ///   Description: Example implementation of Entity Framework Core.
    ///                 http://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx  
    /// </summary>
    public class Account : BaseEntity
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        public DateTime OpenDate { get; set; }
        public Decimal Balance { get; set; }
    }
}
