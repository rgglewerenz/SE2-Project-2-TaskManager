using DataAcess;
using DatabaseInterop;
using DatabaseInterop.Models;
using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBAL
{
    public class UserBAL
    {
        private readonly UsersDA _usersDA;


        private IConfiguration config;

        public UserBAL(IConfiguration config)
        {
            this.config = config;
            _usersDA = new UsersDA(new UnitOfWork(), config);
        }

        #region Get
        public List<UserModal> GetUsers()
        {
            return _usersDA.GetUsers();
        }
        public List<UserTransferModal> GetTransferModalUsers()
        {
            return _usersDA.GetTransferModalUsers();
        }
        public UserModal GetUsernameByID(int id)
        {
            return _usersDA.GetUserByID(id);
        }
        public UserModal GetUsernameByUsername(string username)
        {
            return _usersDA.GetUserByUsername(username);
        }

      
        #endregion Get

        #region Add

        public async Task<bool> AddUser(UserModal user)
        {
            return await _usersDA.AddUser(user);
        }

        #endregion Add

        #region Auth

        public bool AuthorizeUser(string username, string password)
        {
            return _usersDA.AuthorizeUser(username, password);
        }

        public bool ValidateEmail(int userID, string code)
        {
            return _usersDA.ValidateEmail(userID, code);
        }

        public async Task<bool> RequestNewEmailValidationCode(int userID)
        {
            try
            {
                return await _usersDA.RequestNewEmailValidationCode(userID);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public UserTransferModal GetUserTransferByUsername(string username)
        {
            return _usersDA.GetUserTransferByUsername(username);
        }

        public async Task<bool> RequestNewPasswordCode(string email)
        {
            return await _usersDA.RequestNewPasswordCode(email);
        }

        public bool ResetPassword(string pass_reset_code, string new_pass)
        {
            return _usersDA.ResetPassword(pass_reset_code, new_pass);
        }

        public string GetApiCodeForUser(int userID)
        {
            return _usersDA.GetApiCodeForUser(userID);
        }

        public UserTransferModal GetUserByApiCode(string code)
        {
            return _usersDA.GetUserByApiCode(code);
        }

        public bool CheckIfValid(string username, string email)
        {
            return _usersDA.CheckIfValid(username, email);
        }

        public async Task<bool> TestMailer(string to, string subject, string body)
        {
            return await _usersDA.TestMailer(to, subject, body);
        }

        #endregion Auth
    }
}
