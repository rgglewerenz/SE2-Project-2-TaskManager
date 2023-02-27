namespace TaskManagerGUI.Authentication
{
    public interface IAuthProvider
    {
        Task<bool> CheckAuth(string username, string password);
    }
}
