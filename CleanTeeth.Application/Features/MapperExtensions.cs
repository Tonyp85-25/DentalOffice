using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Application.Features.Dentists.Command.CreateDentist;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Features;

internal static class MapperExtensions
{
    public static DentalOfficeDetailDTO ToDTO(this DentalOffice dentalOffice)
    {
        var dto = new DentalOfficeDetailDTO
        {
            Id = dentalOffice.Id,
            Name = dentalOffice.Name,
        };
        return dto;
    }
    
}