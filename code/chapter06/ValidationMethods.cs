using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace chapter06
{
    public static class ValidationMethods
    {
        public static ValidationResult ValidateEmail(string email, ValidationContext context)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    return new ValidationResult("Invalid email", new[] { context.MemberName });
                }
            }

            return ValidationResult.Success;
        }
    }
}
