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
        [StringLength(20, MinimumLength =5, ErrorMessage = "Username must be shorter then 20 characters, but greater than 5")]
        public string UserName { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [Range(18,int.MaxValue, ErrorMessage = "You must be 18 or older to sign up")]
        public int Age { get; set; }

        [Required]
        [PasswordValidation]
        [StringLength(30, MinimumLength = 10, ErrorMessage = "Password must be shorter then 30 characters, but greater then 10 characters")]
        public string Password { get; set; }

    };
}
