using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank4Us.Common.CanonicalSchema;
using Bank4Us.DataAccess.Core;
using Bank4Us.BusinessLayer.Core;
using Microsoft.Extensions.Logging;
using Bank4Us.Common.Core;
using Bank4Us.Common.Facade;
using Microsoft.EntityFrameworkCore;
using NRules;
using NRules.Fluent;
using NRules.RuleModel;

namespace Bank4Us.BusinessLayer.Managers.CustomerManagement
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2021
    ///   Name: William J Leannah
    ///   Description: Example implementation of a Business Layer              
    /// </summary>
    /// 

    public class CustomerManager : BusinessManager, ICustomerManager
    {
        private IRepository _repository;
        private NRules.ISession _businessRulesEngine;
        private ILogger<CustomerManager> _logger;
        private IUnitOfWork _unitOfWork;
        
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        public CustomerManager(IRepository repository, ILogger<CustomerManager> logger, IUnitOfWork unitOfWork, NRules.ISession businessRulesEngine) : base()
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _businessRulesEngine = businessRulesEngine;
            
        }

        public virtual Customer GetCustomer(int customerId)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.GET_ITEM, "The customer Id is " + customerId.ToString());
                return _repository.All<Customer>().Where(c => c.Id == customerId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///   INFO: Create new customer with BRE Example.
        ///          https://github.com/NRules/NRules/wiki/Getting-Started
        /// </summary>
        /// <param name="entity"></param>
        public void Create(BaseEntity entity)
        {
            Customer customer = (Customer)entity;
            _logger.LogInformation("Creating record for {0}", this.GetType());

            //INFO: Fact assertion.
            _businessRulesEngine.Insert(customer);
            //INFO: Execute the business rules.  
            _businessRulesEngine.Fire();
            _logger.LogInformation("Executing the business rules for {0}", this.GetType());

            _repository.Create<Customer>(customer);
            _unitOfWork.SaveChanges();
            _logger.LogInformation("Record saved for {0}", this.GetType());
        
        }

        /// <summary>
        ///   INFO: Update customer with  BRE example.
        ///          https://github.com/NRules/NRules/wiki/Getting-Started
        /// </summary>
        /// <param name="entity"></param>
        public void Update(BaseEntity entity)
        {
            Customer customer = (Customer)entity;
            _logger.LogInformation("Updating record for {0}", this.GetType());

            //INFO: Fact assertion.
            //_businessRulesEngine.Insert(customer);
            //INFO: Execute the business rules.  
            //_businessRulesEngine.Fire();
            _logger.LogInformation("Executing the business rules for {0}", this.GetType());

            _repository.Update<Customer>(customer);
            _unitOfWork.SaveChanges();
            _logger.LogInformation("Record saved for {0}", this.GetType());
        }

        public void Delete(BaseEntity entity)
        {
            Customer customer = (Customer)entity;
            _logger.LogInformation("Updating record for {0}", this.GetType());
            _repository.Delete<Customer>(customer);
            SaveChanges();
            _logger.LogInformation("Record deleted for {0}", this.GetType());
        }

        public IEnumerable<BaseEntity> GetAll()
        {
            return _repository.All<Customer>().ToList<BaseEntity>();
        }

        /// <summary>
        ///   INFO: Return the entire object hierarchy example.  
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            //INFO: EF Lazy loading, make sure to include sub-objects.  E.g. Accounts.
            return _repository.All<Customer>().Include(c=>c.Accounts).ToList(); 
        }

        public void SaveChanges()
        {
            _unitOfWork.SaveChanges();
        }
    }
}
