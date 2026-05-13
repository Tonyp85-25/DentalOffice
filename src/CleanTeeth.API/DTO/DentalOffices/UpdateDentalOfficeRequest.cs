namespace CleanTeeth.API.DTO.DentalOffices;

public record UpdateDentalOfficeRequest()
{
    
    public string Name { get; init; }
    
    public string Number { get; init; }
    
    public string Street { get; init; }
    
    public string Zipcode { get; init; }
    
    public string City { get; init; }
    
    public int OpeningDays { get; init; }
}