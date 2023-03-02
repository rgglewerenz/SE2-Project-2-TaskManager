using Blazored.LocalStorage;
using DTO;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace TaskManagerGUI.Authentication
{
    public interface IAuthProvider
    {
        public UserTransferModal? UserInfo { get; }

        public Task<bool> CheckAuth(string username, string password, ProtectedLocalStorage localStorage);
        Task<bool> CheckIfUserEmailValid(string username);
        public Task CheckLocalStorage(ProtectedLocalStorage localStorage);
        Task CreateAccount(UserTransferModal userInfo, string password);
        public Task Logout(ProtectedLocalStorage localStorage);
        public Task PutInLocalStorage(ProtectedLocalStorage localStorage);
        Task RequestNewEmailValidationCode(string username);
        Task RequestNewPasscode(string email);
        Task ResetPassword(string code, string new_pass);
        Task ValidateEmail(string code);
    }
}
