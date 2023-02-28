using DTO;

namespace TaskManagerGUI.Authentication
{
    public interface IAuthProvider
    {
        public UserTransferModal? UserInfo { get; }

        public Task<bool> CheckAuth(string username, string password);

        public void Logout();
    }
}
