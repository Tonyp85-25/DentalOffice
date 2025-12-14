using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using CleanTeeth.Application.Features.Dentists.Command.CreateDentist;

namespace CleanTeeth.API.DTO.Dentists;

public record CreateDentistDTO()
{
    public CreateDentistDTO(string lastName, string firstName, string email) : this()
    {
        LastName = lastName;
        FirstName = firstName;
        Email = email;
    }
    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public string LastName { get; set; }
    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public string FirstName { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(60)]
    public string Email { get; set; }
    
    public List<string> Offices { get; set; } = new();

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