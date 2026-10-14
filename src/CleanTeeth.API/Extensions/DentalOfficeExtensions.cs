using CleanTeeth.API.BackOffice.DentalOffices;
using CleanTeeth.API.Common.DTO;
using CleanTeeth.API.PatientApp.DentalOffices;
using CleanTeeth.Application.Features.BackOffice.DentalOffices.CreateDentalOffice;
using CleanTeeth.Application.Features.PatientApp.DentalOffices.SearchDentalOffice;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.API.Extensions;

internal static class DentalOfficeExtensions
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
            Days = dto.Days.Count == 0 ? null: dto.Days.Sum(),
        };
    }

    public static DentalOfficeDetailDto ToDto(this DentalOffice office)
    {
        return new DentalOfficeDetailDto
        {
            City = office.Address.City,
            Street = office.Address.Street,
            Zipcode = office.Address.Zipcode,
            Number = office.Address.Number,
            Name = office.Name,
            Id = office.Id
        };
    }

}
