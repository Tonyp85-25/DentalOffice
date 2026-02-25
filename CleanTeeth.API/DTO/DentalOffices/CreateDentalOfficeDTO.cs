using System.ComponentModel.DataAnnotations;

namespace CleanTeeth.API.DTO.DentalOffices;

public class CreateDentalOfficeDTO
{
    [Required]
    [StringLength(150)]
    public required string Name { get; set; }
    
    [Required]
    [StringLength(150)]
    public required string Street { get; set; } 
    
    [Required]
    [StringLength(5)]
    public required string Zipcode { get; set; } 
    
    [Required]
    [StringLength(50)]
    public required string City { get; set; }
    
    [StringLength(5)]
    public string? Number { get; set; }
    
    
    
}