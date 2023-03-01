using System.ComponentModel.DataAnnotations;
namespace DTO
{
    public class UserTransferModal
    {
        [Required]
        [StringLength(20, ErrorMessage = "Username must be shorter then 20 characters")]
        public string UserName { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        public int Age { get; set; }
        public int UserID { get; set; }
        public bool IsValid { get; set; }
    }
}