using CleanTeeth.API.DTO.DentalOffices;
using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeeth.Application.Features.DentalOffices.Queries;

namespace CleanTeeth.API.Extensions;

internal static class APIExtensions
{
    public static CreateDentalOfficeCommand ToCommand(this CreateDentalOfficeDTO dto)
    {
        return new CreateDentalOfficeCommand
        {
            Name = dto.Name,
            Number = dto.Number,
            Street = dto.Street,
            Zipcode = dto.Zipcode,
            City = dto.City,
            OpeningDays = dto.Days.Sum()

        };
    }

    public static GetDentalOfficeSearchQuery ToQuery(this  SearchDentalOfficeDTO dto)
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