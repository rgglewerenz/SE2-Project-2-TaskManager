using Blazored.LocalStorage;
using DTO;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace TaskManagerGUI.Authentication
{
    public interface IAuthProvider
    {
        public UserTransferModal? UserInfo { get; }

        public Task<bool> CheckAuth(string username, string password, ProtectedLocalStorage localStorage);
        public Task CheckLocalStorage(ProtectedLocalStorage localStorage);
        Task CreateAccount(UserTransferModal userInfo, string password);
        public Task Logout(ProtectedLocalStorage localStorage);
        public Task PutInLocalStorage(ProtectedLocalStorage localStorage);
    }
}
