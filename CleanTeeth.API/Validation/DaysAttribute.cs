using System.ComponentModel.DataAnnotations;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.API.Validation;

internal class DaysAttribute : ValidationAttribute
{
    
    public string GetErrorMessage() => "Days are invalid";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var days = (int[])value;
        if (days.Length == 0)
        {
            return ValidationResult.Success;
        }

        var result = days.All((d) => int.IsPow2(d));
        if (result == false)
        {
            return new ValidationResult(GetErrorMessage());
        }
        return ValidationResult.Success;
    }
}