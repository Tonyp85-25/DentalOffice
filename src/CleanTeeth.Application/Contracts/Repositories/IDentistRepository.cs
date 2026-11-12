using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Contracts.Repositories;

public interface IDentistRepository:IRepository<Dentist>
{
    public Task<bool> Exists(string email);
}
