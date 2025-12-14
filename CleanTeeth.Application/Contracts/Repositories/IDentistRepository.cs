using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Application.Contracts.Repositories;

public interface IDentistRepository:IRepository<Dentist>
{
    public Task<bool> Exists(string email);
}