﻿using DatabaseInterop.Models;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly MainContext dbcontext;

        private DbContextTransaction _transaction;

        public UnitOfWork()
        {
            try{
                SqlProviderServices _ = SqlProviderServices.Instance;
                dbcontext= new MainContext();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #region Private Repos

        private GenericRepository<UserModal> _userRepository;

        private GenericRepository<TaskModal> _taskRepository;

        private GenericRepository<UserTaskMap> _userTaskMapRepository;

        private GenericRepository<EmailValidationModal> _emailValidationRepository;

        private GenericRepository<TaskRecurrenceModal> _taskTransferRepository;

        private GenericRepository<PasswordResetModal> _passwordResetModalRepository;

        private GenericRepository<ApiAuthCodeTableModel> _apiAuthCodeRepository;

        #endregion Private Repos

        #region Public Repos

        public GenericRepository<UserModal> UserRepository
        {
            get
            {
                return _userRepository ?? (_userRepository = new GenericRepository<UserModal>(dbcontext));
            }
        }

        public GenericRepository<TaskModal> TaskRepository
        {
            get
            {
                return _taskRepository ?? (_taskRepository = new GenericRepository<TaskModal>(dbcontext));
            }
        }

        public GenericRepository<UserTaskMap> UserTaskMapRepository
        {
            get
            {
                return _userTaskMapRepository ?? (_userTaskMapRepository = new GenericRepository<UserTaskMap>(dbcontext));
            }
        }

        public GenericRepository<EmailValidationModal> EmailValidationRepository
        {
            get
            {
                return _emailValidationRepository ?? (_emailValidationRepository = new GenericRepository<EmailValidationModal>(dbcontext));
            }
        }

        public GenericRepository<TaskRecurrenceModal> TaskRecurranceRepository
        {
            get
            {
                return _taskTransferRepository ?? (_taskTransferRepository = new GenericRepository<TaskRecurrenceModal>(dbcontext));
            }
        }

        public GenericRepository<PasswordResetModal> PasswordResetRepository
        {
            get
            {
                return _passwordResetModalRepository ?? (_passwordResetModalRepository = new GenericRepository<PasswordResetModal>(dbcontext));
            }
        }

        public GenericRepository<ApiAuthCodeTableModel> ApiAuthCodeRepository
        {
            get
            {
                return _apiAuthCodeRepository ?? (_apiAuthCodeRepository = new GenericRepository<ApiAuthCodeTableModel>(dbcontext));
            }
        }
    #endregion  Public Repos

    public void Commit()
        {
            try
            {
                dbcontext.SaveChanges();
                _transaction.Commit();
            }
            catch(Exception ex)
            {
                _transaction.Rollback();
                throw ex;
            }
        }
        
        public void CommitTransaction()
        {
            try
            {
                _transaction.Commit();
            }
            catch(Exception ex)
            {
                _transaction.Rollback();
                throw ex;
            }
        }

        public IEnumerable<T> ExecuteReadQuery<T>(string query) where T : class
        {
            return dbcontext.Database.SqlQuery<T>(query).ToList();
        }

        public MainContext GetContext()
        {
            return dbcontext;
        }

        public void RollbackTransaction()
        {
            try
            {
                _transaction.Rollback(); 
            }
            catch(Exception ex) {
                throw ex;
            }
        }

        public void Save()
        {
            dbcontext.SaveChanges();
        }

        public void StartTransaction()
        {
            _transaction = dbcontext.Database.BeginTransaction();
        }

        #region Dispose
        private bool Disposed;
        public void Dispose()
        {
            if(_transaction != null)
            {
                Commit();
            }
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        protected virtual void Dispose(bool disposing)
        {
            if(!Disposed)
            {
                if (disposing)
                {
                    dbcontext.Dispose();
                }
            }
            Disposed = true;
        }
        #endregion Dispose


    }
}
