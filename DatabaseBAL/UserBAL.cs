using DataAcess;
using DatabaseInterop;
using DatabaseInterop.Models;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBAL
{
    public class UserBAL
    {
        private readonly UsersDA _usersDA = new UsersDA(new UnitOfWork());

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

        public void AddUser(UserModal user)
        {
            _usersDA.AddUser(user);
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

        public bool RequestNewEmailValidationCode(int userID)
        {
            try
            {
                _usersDA.RequestNewEmailValidationCode(userID);
                return true;
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

        public bool RequestNewPasswordCode(string email)
        {
            return _usersDA.RequestNewPasswordCode(email);
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

        #endregion Auth
    }
}
