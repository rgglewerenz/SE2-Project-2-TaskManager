using DatabaseBAL;
using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TasksManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserBAL _userBAL;

        public UsersController(IConfiguration _config)
        {
            _userBAL = new UserBAL(_config);
        }

        #region Get

        [HttpGet("GetUsers")]
        public List<UserTransferModal> GetUsers()
        {

            var host = Dns.GetHostEntry(Dns.GetHostName());
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
