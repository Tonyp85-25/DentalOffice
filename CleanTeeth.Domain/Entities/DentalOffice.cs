using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Domain.Entities;

public class DentalOffice
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    
    public required Address Address { get;  set; }

    private DentalOffice()
    {
      
    }
    
    public static DentalOffice Create(string name, Guid id, Address address)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException($" The {nameof(name)} is required");
        }

        return new DentalOffice
        {
            Id = id,
            Name = name,
            Address = address
        };
    }
}