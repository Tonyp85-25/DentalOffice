using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.Persistence.Repositories;

public class DentalOfficeRepository: Repository<DentalOffice>, IDentalOfficeRepository
{
    public DentalOfficeRepository(CleanTeethDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DentalOffice>> GetAllBy(DentalOfficeCriteria criteria)
    {
        var query = _context.DentalOffices.AsQueryable();
        if (!string.IsNullOrWhiteSpace(criteria.Name))
        {
            query = query.Where((d) => d.Name.Contains(criteria.Name));
        }
        if (!string.IsNullOrWhiteSpace(criteria.Zipcode))
        {
            query = query.Where((d) => d.Address.Zipcode.Equals(criteria.Zipcode));
        }
        if (!string.IsNullOrWhiteSpace(criteria.City))
        {
            query = query.Where((d) => d.Address.City.Equals(criteria.City));
        }

        if (criteria.Days != null)
        {
            var days = (Days)criteria.Days;
                
            query = query.Where((d) => (d.OpeningDays & days)==days);
        }

        return  await query.ToListAsync();

    }
}