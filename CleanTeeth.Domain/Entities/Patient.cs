using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Domain.Entities;

public class Patient
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;

    public Patient(string name, Email email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException($" The {nameof(name)} is required");
        }

        if (email is null)
        {
            throw new BusinessRuleException($" The {nameof(email)} is required");
        }
        
        Name = name;
        Id = Guid.NewGuid();
        Email = email;
    }
}