using Bank4Us.Common.Core;
using Bank4Us.Common.CanonicalSchema;
using Bank4Us.DataAccess.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank4Us.BusinessLayer.Core
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2023
    ///   Name: William J Leannah
    ///   Description: Example implementation of a Business Layer              
    /// </summary>
    /// 
    public interface IActionManager
    {
        void Create(BaseEntity entity);
        void Update(BaseEntity entity);
        void Delete(BaseEntity entity);
        IEnumerable<BaseEntity> GetAll();
        IUnitOfWork UnitOfWork { get; }
        void SaveChanges();
    }
}
