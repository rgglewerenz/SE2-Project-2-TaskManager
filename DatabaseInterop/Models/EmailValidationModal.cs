using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models
{
    public class EmailValidationModal
    {
        public int EmailValidationID { get; set; }
        public int UserID { get; set; }
        public bool IsValid { get; set; }
        public string? ActivationCode { get; set; }
        public DateTime? ActivationCodeCreation { get; set; } 

    }
}
