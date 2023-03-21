using DatabaseBAL;
using DatabaseInterop.Models;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace TasksManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserBAL _userBAL;


        public AuthController(IConfiguration _config) {
            _userBAL = new UserBAL(_config);
        }

        #region Get

        [HttpGet("TestMailer")]
        public async Task<bool> TestMailer(string to, string subject, string body)
        {
            return await _userBAL.TestMailer(to, subject, body);
        }

        [HttpGet("TestPassResetEmail")]
        public async Task<bool> TestPassResetEmail(string to)
        {
            return await _userBAL.TestPassResetEmail(to);
        }

        [HttpGet("CheckIfUserEmailValid")]
        public bool CheckIfUserEmailValid(string username)
        {
            return _userBAL.CheckIfUserEmailValid(username);
        }

        [HttpGet("AuthUser")]
        public bool AuthUser(string username, string password)
        {
            return _userBAL.AuthorizeUser(username, password);
        }

        [HttpGet("RequestNewPasswordCode")]
        public async Task<bool> RequestNewPasswordCode(string email)
        {
            return await _userBAL.RequestNewPasswordCode(email);
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
        public async Task<bool> CreateUser(string username, string password, int age, string email) {
            UserModal new_user = new UserModal()
            {
                Age= age,
                Email= email,
                UserName= username,
            };
            new_user.CreatePasswordHash(password);
            return await _userBAL.AddUser(new_user);
        }

        [HttpPost("ValidateEmail")]
        public bool ValidateEmail(string code)
        {
            return _userBAL.ValidateEmail(code);
        }

        [HttpPost("RequestNewEmailValidationCode")]
        public async Task<bool> RequestNewEmailValidationCode(string username)
        {
            return await _userBAL.RequestNewEmailValidationCode(username);
        }

        #endregion Post

    }
}
