using CleanTeeth.Application.Utilities;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Application.Features.DentalOffices.Queries;

public class GetDentalOfficeDetailQuery : IRequest<DentalOfficeDetailDto>
{
    public required Guid Id { get; set; }
}

public class DentalOfficeDetailDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Street { get; set; }
    public required string Zipcode { get; set; }
    public required string City { get; set; }
    public required string? Number { get; set; }

}
