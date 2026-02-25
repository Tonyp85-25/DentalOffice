using CleanTeeth.Domain.Exceptions;

namespace CleanTeeth.Domain.ValueObjects;


public record Address
{
    private Address()
    {
        
    }

    public string Number { get; set; } = string.Empty;
    public required string Street { get; set; }
    public required string Zipcode { get; set; }
    public required string City { get; set; }
    public  string? AdditionalInfos { get; set; }

    private static bool IsZipcodeValid(string zipcode)
    {
        return zipcode.Length == 5 && int.TryParse(zipcode, out int result);
    }

    private static bool IsCityValid(string city)
    {
        return city.Length > 2;
    }

    private static bool IsStreetValid(string street)
    {
        return street.Length > 5;
    }

    public static Address Create(string? number, string street, string zipcode, string city, string? additional=null)
    {
        if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(zipcode) || string.IsNullOrWhiteSpace(city))
        {
            throw new BusinessRuleException(
                $" The names of {nameof(street)}, {nameof(zipcode)}, {nameof(city)} are required");
        }

        if (!IsCityValid(city))
        {
            throw new BusinessRuleException(
                $"{nameof(city)} must be at least 2 characters");
        }
        
        if (!IsStreetValid(street))
        {
            throw new BusinessRuleException(
                $"{nameof(street)} must be at least 5 characters");
        }

        if (!IsZipcodeValid(zipcode))
        {
            throw new BusinessRuleException(
                $"{nameof(zipcode)} must be 5 digit");
        }

        return new Address
        {
            Number = number,
            City = city,
            Zipcode = zipcode,
            Street = street,
            AdditionalInfos = additional
        };
    }
    
    public override string ToString()
    {
        return $"{Number};{Street};{Zipcode};{City}";
    }
    
    public static Address FromString(string address)
    {
        var expr = address.Split(';');
        return Address.Create(expr[0], expr[1], expr[2], expr[3]);
    }
}