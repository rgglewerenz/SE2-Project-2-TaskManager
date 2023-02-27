namespace DTO
{
    public class UserTransferModal
    {
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public int Age { get; set; }
        public int UserID { get; set; }
        public bool IsValid { get; set; }
    }
}