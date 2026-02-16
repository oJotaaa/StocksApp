using System.ComponentModel.DataAnnotations;

namespace StocksApp.ServiceContracts.CustomValidators
{
    public class MinimumDateAttribute : ValidationAttribute
    {
        override protected ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue < new DateTime(2000, 1, 1))
                {
                    return new ValidationResult("Date must be on or after January 1, 2000.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
