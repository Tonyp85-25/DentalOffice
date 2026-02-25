using CleanTeeth.API.DTO.DentalOffices;
using CleanTeeth.API.DTO.Dentists;
using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeeth.Application.Features.Dentists.Command.CreateDentist;

namespace CleanTeeth.API.Extensions;

internal static class APIExtensions
{
    public static CreateDentistCommand FromDto(this CreateDentistCommand command, CreateDentistDTO dto)
    {
        List<Guid> offices = new();
        foreach (var office in dto.Offices)
        {
            if (Guid.TryParse(office,out Guid value))
            {
                offices.Add(value);
            }
        }

        
        return new CreateDentistCommand
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Offices = offices
        };
    }

    public static CreateDentalOfficeCommand ToCommand(this CreateDentalOfficeDTO dto)
    {
        return new CreateDentalOfficeCommand
        {
            Name = dto.Name,
            Number = dto.Number,
            Street = dto.Street,
            Zipcode = dto.Zipcode,
            City = dto.City

        };
    }
}