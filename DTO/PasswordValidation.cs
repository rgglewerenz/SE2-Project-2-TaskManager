using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    sealed public class PasswordValidation : ValidationAttribute
    {
        public PasswordValidation()
        {
        }

        public override bool IsValid(object value)
        {
            return CheckPassComplexity((String)value);
        }

        private bool CheckPassComplexity(string password)
        {
            if (!password.Any(char.IsDigit))
            {
                ErrorMessage = "Password must conatin a number";
                return false;
            }
            if (!password.Any(char.IsLetter))
            {
                ErrorMessage = "Password must conatin a letter";
                return false;
            }
            if (!password.Any(char.IsPunctuation) && !password.Any(char.IsSymbol))
            {
                ErrorMessage = "Password must conatin a special character";
                return false;
            }
            return true;
        }
    }
}
