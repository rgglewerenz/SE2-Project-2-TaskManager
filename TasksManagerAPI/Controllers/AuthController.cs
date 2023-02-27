using DatabaseBAL;
using DatabaseInterop.Models;
using Microsoft.AspNetCore.Mvc;

namespace TasksManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserBAL _userBAL = new UserBAL();

        [HttpGet("AuthUser")]
        public bool AuthUser(string username, string password)
        {
            return _userBAL.AuthorizeUser(username, password);
        }

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
    }
}
