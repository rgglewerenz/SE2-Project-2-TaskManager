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
        [StringLength(20, ErrorMessage = "Username must be shorter then 20 characters")]
        public string UserName { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public int Age { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Password must be shorter then 30 characters")]
        public string Password { get; set; }

    };
}
