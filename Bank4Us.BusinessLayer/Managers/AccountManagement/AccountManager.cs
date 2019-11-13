using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank4Us.Common.CanonicalSchema;
using Bank4Us.BusinessLayer.Core;
using Bank4Us.DataAccess.Core;
using Microsoft.Extensions.Logging;
using Bank4Us.Common.Facade;
using Bank4Us.Common.Core;
using Bank4Us.BusinessLayer.Managers.CustomerManagement;

namespace Bank4Us.BusinessLayer.Managers.AccountManagement
{

    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2019
    ///   Name: William J Leannah
    ///   Description: Example implementation of a Business Layer              
    /// </summary>
    /// 
    public class AccountManager : BusinessManager , IAccountManager
    {
        private IRepository _repository;
        private ILogger<AccountManager> _logger;
        private IUnitOfWork _unitOfWork;
        private ICustomerManager _serviceRequestManager;

       
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        public AccountManager(IRepository repository, ILogger<AccountManager> logger,  IUnitOfWork unitOfWork, ICustomerManager serviceRequestManager) : base()
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _serviceRequestManager = serviceRequestManager;
        }

        public virtual Account GetAccount(int accountId)
        {
            try 
            {
                _logger.LogInformation(LoggingEvents.GET_ITEM, "The account Id is " + accountId.ToString());
                return _repository.All<Account>().Where(a => a.Id == accountId).FirstOrDefault();
            }catch(Exception ex)
            {
                throw ex;
            }
               
        }

        public void Create(BaseEntity entity)
        {
            Account account = (Account)entity;
            _logger.LogInformation("Creating record for {0}", this.GetType());
            _repository.Create<Account>(account);
            SaveChanges();
            _logger.LogInformation("Record saved for {0}", this.GetType());
        }

        public void Update(BaseEntity entity)
        {
            Account account = (Account)entity;
            _logger.LogInformation("Updating record for {0}", this.GetType());
             _repository.Update<Account>(account);
            SaveChanges();
            _logger.LogInformation("Record saved for {0}", this.GetType());
        }

        public void Delete(BaseEntity entity)
        {
            Account account = (Account)entity;
            _logger.LogInformation("Updating record for {0}", this.GetType());
            _repository.Delete<Account>(account);
            SaveChanges();
            _logger.LogInformation("Record deleted for {0}", this.GetType());
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _repository.All<Account>().ToList();
        }

        public IEnumerable<BaseEntity> GetAll()
        {
            return _repository.All<Account>().ToList<BaseEntity>();
        }

        public void SaveChanges()
        {
            _unitOfWork.SaveChanges();
        }
        
    }
}
