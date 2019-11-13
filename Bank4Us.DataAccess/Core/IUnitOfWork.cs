using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank4Us.DataAccess.Core
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2019
    ///   Name: William J Leannah
    ///   Description: Example implementation of Entity Framework Core.
    ///                 http://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx  
    /// </summary>
    public interface IUnitOfWork
    {
        void BeginTransaction();

        void RollbackTransaction();

        void CommitTransaction();

        void SaveChanges();
    }
}
