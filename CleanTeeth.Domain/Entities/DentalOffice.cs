using CleanTeeth.Domain.Exceptions;

namespace CleanTeeth.Domain.Entities;

public class DentalOffice
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;

    public DentalOffice(string name, Guid id)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException($" The {nameof(name)} is required");
        }
        Name = name;
        Id = id ;
    }
}