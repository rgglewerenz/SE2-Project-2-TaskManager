using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CreateUserTransferModal
    {
        [Required]
        [StringLength(20, MinimumLength =5, ErrorMessage = "Username must be between 5, and 20 characters long")]
        public string UserName { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [Range(18,int.MaxValue, ErrorMessage = "You must be 18 or older to sign up")]
        public int Age { get; set; }

        [Required]
        [PasswordValidation]
        [StringLength(30, MinimumLength = 10, ErrorMessage = "Password must be between 10, and 30 characters long")]
        public string Password { get; set; }

    };
}
