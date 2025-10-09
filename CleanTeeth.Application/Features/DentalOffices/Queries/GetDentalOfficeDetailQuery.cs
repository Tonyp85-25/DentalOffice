using CleanTeeth.Application.Utilities;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Features.DentalOffices.Queries;

public class GetDentalOfficeDetailQuery : IRequest<DentalOfficeDetailDTO>
{
    public required Guid Id { get; set; }
}

public class DentalOfficeDetailDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}