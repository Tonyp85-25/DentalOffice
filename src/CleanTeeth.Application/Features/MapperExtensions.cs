using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Application.Features.Dentists.Command.CreateDentist;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Features;

internal static class MapperExtensions
{
    public static DentalOfficeDetailDto ToDto(this DentalOffice dentalOffice)
    {
        var dto = new DentalOfficeDetailDto
        {
            Id = dentalOffice.Id,
            Name = dentalOffice.Name,
            Street = dentalOffice.Address.Street,
            Zipcode = dentalOffice.Address.Zipcode,
            City = dentalOffice.Address.City,
            Number = dentalOffice.Address.Number
        };
        return dto;
    }

}
