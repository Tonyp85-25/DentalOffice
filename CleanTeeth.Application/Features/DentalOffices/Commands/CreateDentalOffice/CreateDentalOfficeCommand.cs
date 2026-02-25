using CleanTeeth.Application.Utilities;

namespace CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;

public class CreateDentalOfficeCommand: IRequest<Guid>
{
    public required string Name { get; set; }
    public string Number { get; set; }
    public required string Street { get; set; }
    public required string Zipcode { get; set; }
    public required string City { get; set; }
}