using FluentValidation.Results;

namespace CleanTeeth.Application.Exceptions;

public class CustomValidationException: Exception
{
    public List<string> ValidationErrors { get; set; } = [];
    
    private readonly ValidationResult _validationResult;

    public CustomValidationException(ValidationResult validationResult)
    {
        _validationResult = validationResult;
        foreach (var validationError in validationResult.Errors )
        {
            ValidationErrors.Add(validationError.ErrorMessage);
        }
    }
    public IDictionary<string, string[]> ToDictionary() => _validationResult.ToDictionary();
   
}