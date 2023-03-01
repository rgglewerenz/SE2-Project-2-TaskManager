using DatabaseBAL;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace TasksManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserBAL _userBAL = new UserBAL();

        #region Get

        [HttpGet("GetUsers")]
        public List<UserTransferModal> GetUsers()
        {
            return _userBAL.GetTransferModalUsers();
        }

        [HttpGet("GetUser")]
        public UserTransferModal GetUser(string username)
        {
            return _userBAL.GetUserTransferByUsername(username);
        }

        #endregion Get

    }
}
