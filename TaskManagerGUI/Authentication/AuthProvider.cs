using System.Web;
using TaskManagerGUI.Services;
using DTO;

namespace TaskManagerGUI.Authentication
{
    public class AuthProvider : BaseWebApi, IAuthProvider
    {

        private UserTransferModal? _user;

        public UserTransferModal? UserInfo => _user;

        public AuthProvider(HttpClient httpClient, IConfiguration configuration): base(httpClient, configuration) {
            BaseURL = configuration.GetValue<string>("BaseApiUrl");
        }

        public async Task<bool> CheckAuth(string username, string password)
        {
#if DEBUG
            await GetUserInstance(username);
            return true;
#else

            var result = await GetInfoNonClass<bool>(BaseURL + "Auth/AuthUser" + $"?username={HttpUtility.UrlEncode(username)}&password={HttpUtility.UrlEncode(password)}");
            if(_user == null && result)
            {
                await GetUserInstance(username);
            }
            return result;
#endif
        }

        public void Logout()
        {
            _user = null;
        }



        #region Private Methods
        private async Task GetUserInstance(string username)
        {
#if DEBUG
            _user = new UserTransferModal() { Age = int.MaxValue, Email = "rgglewerenz@gamil.com", IsValid = true, UserID = 1, UserName = "Rlewerenz" };
#else
            _user = await GetInfoFromJson<UserTransferModal>(BaseURL + "Users/GetUser" + $"?username={HttpUtility.UrlEncode(username)}");
#endif
        }

        
        #endregion Private Methods
    }
}
