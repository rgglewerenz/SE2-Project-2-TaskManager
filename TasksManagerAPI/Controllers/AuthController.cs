using DatabaseBAL;
using DatabaseInterop.Models;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace TasksManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserBAL _userBAL = new UserBAL();

        #region Get

        [HttpGet("AuthUser")]
        public bool AuthUser(string username, string password)
        {
            return _userBAL.AuthorizeUser(username, password);
        }

        [HttpGet("RequestNewPasswordCode")]
        public bool RequestNewPasswordCode(int UserID)
        {
            return _userBAL.RequestNewPasswordCode(UserID);
        }

        [HttpGet("ResetPassword")]
        public bool ResetPassword(string pass_reset_code, string new_pass)
        {
            return _userBAL.ResetPassword(pass_reset_code, new_pass);
        }

        [HttpGet("GetApiCodeForUser/{UserID:int}")]
        public string GetApiCodeForUser(int UserID)
        {
            return _userBAL.GetApiCodeForUser(UserID);
        }

        [HttpGet("GetUserByApiCode")]
        public UserTransferModal GetUserByApiCode(string Code)
        {
            return _userBAL.GetUserByApiCode(Code);
        }

        [HttpGet("CheckIfValid")]
        public bool CheckIfValid(string username, string email)
        {
            return _userBAL.CheckIfValid(username, email);
        }

        #endregion Get

        #region Post

        [HttpPost("CreateUser")]
        public bool CreateUser(string username, string password, int age, string email) {
            UserModal new_user = new UserModal()
            {
                Age= age,
                Email= email,
                UserName= username,
            };
            new_user.CreatePasswordHash(password);
            _userBAL.AddUser(new_user);
            return true;
        }

        [HttpPost("ValidateEmail")]
        public bool ValidateEmail(int userID, string code)
        {
            return _userBAL.ValidateEmail(userID, code);
        }

        [HttpPost("RequestNewEmailValidationCode")]
        public bool RequestNewEmailValidationCode(int userID)
        {
            return _userBAL.RequestNewEmailValidationCode(userID);
        }

        #endregion Post

    }
}
