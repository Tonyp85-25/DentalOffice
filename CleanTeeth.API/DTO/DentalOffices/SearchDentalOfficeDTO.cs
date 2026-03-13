using System.ComponentModel.DataAnnotations;
using CleanTeeth.API.Validation;
using Microsoft.Extensions.Primitives;

namespace CleanTeeth.API.DTO.DentalOffices;

public record SearchDentalOfficeDTO
{
    [StringLength(50)]
    public   string? Name { get; init; }
    
    
    [RegularExpression(@"^[0-9]{5}$")]
    public   string? Zipcode { get; init; }
    
    [StringLength(30)]
    public   string? City { get; init; }

    [Days]
    public int[] Days{ get; init; }

    public bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Zipcode) && string.IsNullOrWhiteSpace(City) &&
               Days.Length == 0;
    }
    

}