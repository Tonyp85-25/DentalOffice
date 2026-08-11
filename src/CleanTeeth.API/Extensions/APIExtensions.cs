using CleanTeeth.API.DTO.DentalOffices;
using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeeth.Application.Features.DentalOffices.Queries;

namespace CleanTeeth.API.Extensions;

internal static class ApiExtensions
{
    public static CreateDentalOfficeCommand ToCommand(this CreateDentalOfficeDto dto)
    {
        return new CreateDentalOfficeCommand
        {
            Name = dto.Name,
            Number = dto.Number ?? string.Empty,
            Street = dto.Street,
            Zipcode = dto.Zipcode,
            City = dto.City,
            OpeningDays = dto.Days.Sum()

        };
    }

    public static GetDentalOfficeSearchQuery ToQuery(this  SearchDentalOfficeDto dto)
    {
        return new GetDentalOfficeSearchQuery
        {
            City = dto.City,
            Name = dto.Name,
            Zipcode = dto.Zipcode,
            Days = !dto.Days.Any() ? null: dto.Days.Sum(),
        };
    }
}
