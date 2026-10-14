using System.ComponentModel.DataAnnotations;
using CleanTeeth.Application.Features.BackOffice.Dentists.Command.CreateDentist;

namespace CleanTeeth.API.BackOffice.Dentists;

public record CreateDentistDto()
{
    public CreateDentistDto(string lastName, string firstName, string email) : this()
    {
        LastName = lastName;
        FirstName = firstName;
        Email = email;
    }
    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public required string LastName { get; set; }
    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public required string FirstName { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(60)]
    public required string Email { get; set; }

    public IReadOnlyCollection<string> Offices { get; set; } 

    public CreateDentistCommand ToCommand()
    {
        List<Guid> offices = new();
        foreach (var office in  Offices)
        {
            if (Guid.TryParse(office,out Guid value))
            {
                offices.Add(value);
            }
        }

        return new CreateDentistCommand
        {
            Email=Email,
            FirstName = FirstName,
            LastName = LastName,
            Offices = offices
        };
    }

}
