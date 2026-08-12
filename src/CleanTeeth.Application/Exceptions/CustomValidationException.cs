using FluentValidation.Results;

namespace CleanTeeth.Application.Exceptions;

public class CustomValidationException: Exception
{
    public IList<string> ValidationErrors { get;  } = [];

    private readonly ValidationResult _validationResult ;

    public CustomValidationException(ValidationResult validationResult)
    {
        _validationResult = validationResult;
        foreach (var validationError in validationResult.Errors )
        {
            ValidationErrors.Add(validationError.ErrorMessage);
        }
    }

    public CustomValidationException()
    {
    }

    public CustomValidationException(string? message) : base(message)
    {
    }

    public CustomValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public IDictionary<string, string[]> ToDictionary() => _validationResult.ToDictionary();

}
