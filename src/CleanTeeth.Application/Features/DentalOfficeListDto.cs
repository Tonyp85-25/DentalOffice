namespace CleanTeeth.Application.Features;

public record DentalOfficeListDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
