using System.Web;
using TaskManagerGUI.Services;
using DTO;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace TaskManagerGUI.Authentication
{
    public class AuthProvider : BaseWebApi, IAuthProvider
    {

        private UserTransferModal? _user;

        public UserTransferModal? UserInfo => _user;

        bool pingApi = false;

        public AuthProvider(HttpClient httpClient, IConfiguration configuration): base(httpClient, configuration) {
            BaseURL = configuration.GetValue<string>("BaseApiUrl");
            pingApi = configuration.GetValue<bool>("PingApi");
        }

        public async Task<bool> CheckAuth(string username, string password, ProtectedLocalStorage localStorage)
        {
            var result = pingApi ? await GetInfoNonClass<bool>(BaseURL + "Auth/AuthUser" + $"?username={HttpUtility.UrlEncode(username)}&password={HttpUtility.UrlEncode(password)}") : true;
            if (_user == null && result)
            {
                await GetUserInstance(username);
                await PutInLocalStorage(localStorage);
            }
            return result;
        }

        public async Task CheckLocalStorage(ProtectedLocalStorage localStorage)
        {
            if (!pingApi)
            {
                try
                {
                    if ((await localStorage.GetAsync<string>("ApiAuthToken")).Success)
                        await GetUserInstance("");
                }
                catch(Exception ex)
                {

                }
                return;
            }
            try
            {
                if((await localStorage.GetAsync<string>("ApiAuthToken")).Success && _user == null)
                {
                    var code = await localStorage.GetAsync<string>("ApiAuthToken");
                    _user = await GetInfoFromJson<UserTransferModal>(BaseURL + $"Auth/GetUserByApiCode?Code={code.Value}");
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Here");
            }
        }

        public async Task PutInLocalStorage(ProtectedLocalStorage localStorage)
        {
            if(!pingApi || _user == null)
            {
                return;
            }

            var code = await GetInfoString(BaseURL + $"Auth/GetApiCodeForUser/{_user.UserID}");

            if (code == null)
            {
                throw new Exception("Unable to get the Api code");
            }

            await localStorage.SetAsync("ApiAuthToken", code);
        }

        public async Task Logout(ProtectedLocalStorage localStorage)
        {
            _user = null;
            await localStorage.DeleteAsync("ApiAuthToken");
        }

        public async Task CreateAccount(UserTransferModal userInfo, string password)
        {
            if (!pingApi)
            {
                return;
            }


            var isValid = await GetInfoNonClass<bool>(BaseURL +
                $"Auth/CheckIfValid?username={HttpUtility.UrlEncode(userInfo.UserName)}&" +
                $"email={HttpUtility.UrlEncode(userInfo.Email)}");

            if (!isValid)
            {
                throw new Exception("An account with this username/email already exists");
            }

            var result = await PostInfo<dynamic, bool>(
                BaseURL + 
                $"Auth/CreateUser?username={HttpUtility.UrlEncode(userInfo.UserName)}&" +
                $"password={HttpUtility.UrlEncode(password)}&" +
                $"age={userInfo.Age}&" +
                $"email={HttpUtility.UrlEncode(userInfo.Email)}"
                , new { });

            var item = result;

        }

        public async Task RequestNewPasscode(string email)
        {
            var result = await GetInfoNonClass<bool>(BaseURL + $"Auth/RequestNewPasswordCode?email={email}");
            if (!result)
                throw new Exception("Unable to send reset code, please try again later");
        }

        public async Task ResetPassword(string code, string new_pass)
        {
            var result = await GetInfoNonClass<bool>(BaseURL + $"Auth/ResetPassword?pass_reset_code={code}&new_pass={new_pass}");
            if (!result)
                throw new Exception("Unable to reset password, please try again later");
        }

        public async Task ValidateEmail(string code)
        {
            var result = await PostInfo<dynamic,bool>(BaseURL + $"Auth/ValidateEmail?code={code}", new {});
            if (!result)
                throw new Exception("Unable validate the email, please request a new code");
        }


        public async Task RequestNewEmailValidationCode(string username)
        {
            var result = await PostInfo<dynamic, bool>(BaseURL + $"Auth/RequestNewEmailValidationCode?username={username}", new { });
            if (!result)
                throw new Exception("Unable validate the email, please request a new code");
        }

        public async Task<bool> CheckIfUserEmailValid(string username)
        {
            var result = await GetInfoNonClass<bool>(BaseURL + $"Auth/CheckIfUserEmailValid?username={username}");
            return result;
        }

        #region Private Methods
        private async Task GetUserInstance(string username)
        {
            _user = pingApi ? await GetInfoFromJson<UserTransferModal>(BaseURL + "Users/GetUser" + $"?username={HttpUtility.UrlEncode(username)}") : new UserTransferModal() { Age = int.MaxValue, Email = "rgglewerenz@gamil.com", IsValid = true, UserID = 1, UserName = "Rlewerenz" };
        }
        #endregion Private Methods
    }
}
