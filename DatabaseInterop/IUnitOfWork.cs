using DatabaseInterop.Models;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop
{
    public interface IUnitOfWork
    {
        #region Methods
        void Commit();
        void Dispose();
        void Save();
        void StartTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        IEnumerable<T> ExecuteReadQuery<T>(string query) where T : class;
        MainContext GetContext();
        #endregion Methods

        #region Repositories
        public GenericRepository<UserModal> UserRepository { get; }
        public GenericRepository<TaskModal> TaskRepository { get; }
        public GenericRepository<UserTaskMap> UserTaskMapRepository { get; }
        public GenericRepository<EmailValidationModal> EmailValidationRepository { get; }
        public GenericRepository<TaskRecurrenceModal> TaskRecurranceRepository { get; }
        public GenericRepository<PasswordResetModal> PasswordResetModalRepository { get; }
        public GenericRepository<ApiAuthCodeTableModel> ApiAuthCodeRepository { get; }
        #endregion Repositories
    }
}
