using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Tests.Infrastructure;

public class StubDentistRepository:IDentistRepository
{
    public  List<Dentist> Data { get; set; } = new();

    public bool MustThrow = false;
    public Task<Dentist?> GetById(Guid id)
    {
        return Task.FromResult(Data.Find(d => d.Id == id)) ;
    }

    public Task<IEnumerable<Dentist>> GetAll()
    {
        return Task.FromResult<IEnumerable<Dentist>>(Data);
    }

    public Task<Dentist> Add(Dentist entity)
    {
        if (MustThrow)
        {
            Throws();
        }
        Data.Add(entity);
        return Task.FromResult(entity);
    }

    public Task Update(Dentist entity)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Dentist entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exists(string email)
    {
        var result = Data.FirstOrDefault(d => d.Email.Value == email) != null;
        return Task.FromResult(result);
    }

    private void Throws()
    {
        throw new Exception("");
    }
}