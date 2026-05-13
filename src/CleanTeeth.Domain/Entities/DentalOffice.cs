using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Domain.Entities;

public class DentalOffice
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public required Address Address { get;  set; }
    
    public Days OpeningDays { get; private set; }

    private DentalOffice()
    {
      
    }
    
    public static DentalOffice Create(string name, Guid id, Address address, int days)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException($" The {nameof(name)} is required");
        }

        return new DentalOffice
        {
            Id = id,
            Name = name,
            Address = address,
            OpeningDays = (Days) days,
        };
    }

    public DentalOffice UpdateName(string? name)
    {
        Name = name ?? Name;
        return this;
    }
    
    public DentalOffice UpdateAddress(string?number, string? street, string? zipcode, string? city, string? additional)
    {
        this.Address = Address.UpdateAddress(number,  street,  zipcode,  city,  additional);
        return this;
    }
    
    public DentalOffice UpdateOpeningDays(int? days)
    {
        OpeningDays = days ==null ? OpeningDays : (Days) days;
        return this;
    }
}