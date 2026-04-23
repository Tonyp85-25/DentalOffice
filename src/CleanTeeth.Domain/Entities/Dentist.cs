using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Domain.Entities;

public class Dentist
{
    public Guid Id { get;  set; }
    public string LastName { get; set; } = null!;
    public string FirstName { get;  set; } = null!;
    public Email Email { get; set; } = null!;
    

    private List<DentalOffice> DentalOffices { get; set; } = new();

    public bool IsPublished { get; private set; } = false;

    private bool HasDentalOffice => DentalOffices.Count > 0;

    private void Publish()
    {
        IsPublished = true;
    }
    private void Unpublish()
    {
        IsPublished = false;
    }
    

    public void AddDentalOffice(DentalOffice dentalOffice)
    {
        if (!HasDentalOffice)
        {
            Publish();
        }
        DentalOffices.Add(dentalOffice);
        
    }
    public void RemoveDentalOffice(DentalOffice dentalOffice)
    {
        DentalOffices.Remove(dentalOffice);
        if (!HasDentalOffice)
        {
            Unpublish();
        }
    }
    
    

}