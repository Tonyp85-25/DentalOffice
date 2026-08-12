using System.ComponentModel.DataAnnotations;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.API.Validation;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
internal sealed class DaysAttribute : ValidationAttribute
{
    private new const string ErrorMessage ="Days are invalid";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if ( value == null)
        {
            return ValidationResult.Success;
        }

        if (value is List<int> days)
        {
            var result = days.All(int.IsPow2);
            if (!result)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);


    }
}
