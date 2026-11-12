using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.Persistence.Repositories;

public class DentistRepository:Repository<Dentist>, IDentistRepository
{
    public DentistRepository(CleanTeethDbContext context) : base(context)
    {
    }

    public async Task<bool> Exists(string email)
    {
        return await Context.Dentists.AnyAsync(d => d.Email == new Email(email));
    }
}
