namespace CleanTeeth.API.Common.DTO;

public record DentalOfficeDetailDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Street { get; set; }
    public required string Zipcode { get; set; }
    public required string City { get; set; }
    public required string? Number { get; set; }

}
