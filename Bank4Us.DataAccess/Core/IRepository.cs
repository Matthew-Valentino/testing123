using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bank4Us.DataAccess.Core;
using Microsoft.Data.SqlClient;

namespace Bank4Us.DataAccess.Core
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2023
    ///   Name: William J Leannah
    ///   Description: Example implementation of Entity Framework Core.
    ///                 http://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx  
    /// </summary>
    public interface IRepository
    {
        IQueryable<T> All<T>() where T : class;
        void Create<T>(T TObject) where T : class;
        void Delete<T>(T TObject) where T : class;
        void Delete<T>(Expression<Func<T, bool>> predicate) where T : class;
        void Update<T>(T TObject) where T : class;
        void ExecuteProcedure(string procedureCommand, params SqlParameter[] sqlParams);
        IEnumerable<T> Filter<T>(Expression<Func<T, bool>> predicate) where T : class;
        IEnumerable<T> Filter<T>(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50) where T : class;
        T Find<T>(Expression<Func<T, bool>> predicate) where T : class;
        T Single<T>(Expression<Func<T, bool>> expression) where T : class;
        bool Contains<T>(Expression<Func<T, bool>> predicate) where T : class;
    }
}
