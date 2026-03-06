using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Tests.Fixtures;

public class DentalOfficeBuilder
{
    
    private string _name = "Dental Office A";
    private Guid _guid = Guid.Empty;
    private Address _address = Address.FromString("2;rue du quai;11111;Uneville");
    private int _days = 31;

    public DentalOffice Build()
    {
        return DentalOffice.Create(_name,_guid,_address,_days);
    }

    public DentalOfficeBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    public DentalOfficeBuilder WithGuid(Guid guid)
    {
        _guid = guid;
        return this;
    }
    public DentalOfficeBuilder WithAddress(string address)
    {
        _address = Address.FromString(address);
        return this;
    }
}