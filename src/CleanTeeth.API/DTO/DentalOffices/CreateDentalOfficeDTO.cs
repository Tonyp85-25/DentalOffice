using System.ComponentModel.DataAnnotations;

namespace CleanTeeth.API.DTO.DentalOffices;

public class CreateDentalOfficeDto
{
    [Required]
    [StringLength(150)]
    public  string Name { get; set; }

    [Required]
    [StringLength(150)]
    public  string Street { get; set; }

    [Required]
    [RegularExpression("^\\d{5}$",ErrorMessage = "Zipcode must be 5 digits")]
    public  string Zipcode { get; set; }

    [Required]
    [StringLength(50)]
    public  string City { get; set; }

    [StringLength(5)]
    public string? Number { get; set; }

    [Required]
#pragma warning disable CA2227
    public IList<int> Days { get; set; }
#pragma warning restore CA2227

}
