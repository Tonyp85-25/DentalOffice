using CleanTeeth.Domain.Exceptions;

namespace CleanTeeth.Domain.ValueObjects;

public record PhoneNumber
{
    private PhoneNumber()
    {
       
    }

    public static  PhoneNumber  Create(string value)
    {
        if (value.Length != 10)
        {
            throw new BusinessRuleException("PhoneNumber must be 10 characters");
        }

        if (!value.StartsWith("0"))
        {
            throw new BusinessRuleException("PhoneNumber must start with 0");
        }

        return new PhoneNumber { Value = value };
    }

    public required string Value { get; set; }
}