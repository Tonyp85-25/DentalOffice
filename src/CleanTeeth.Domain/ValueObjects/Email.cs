using CleanTeeth.Domain.Exceptions;

namespace CleanTeeth.Domain.ValueObjects;

public record Email()
{
    public string Value { get; } = null!;

    public Email(string value) : this()
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BusinessRuleException($"The {nameof(value)} is required");
        }

        if (!value.Contains("@"))
        {
            throw new BusinessRuleException($" The {nameof(value)} is not valid");
        }
        Value = value;
    }
}