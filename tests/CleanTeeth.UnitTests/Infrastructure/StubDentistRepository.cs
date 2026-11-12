using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Tests.Infrastructure;

public class StubDentistRepository:IDentistRepository
{
    public   IList<Dentist> Data { get; }=new List<Dentist>();

    public bool MustThrow { get; set; }
    public Task<Dentist?> GetById(Guid id)
    {
        return Task.FromResult(Data.FirstOrDefault(d => d.Id == id)) ;
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
        var result = Data.FirstOrDefault(d => d.Email.Value.Equals(email,StringComparison.Ordinal) ) != null;
        return Task.FromResult(result);
    }

    private static void Throws()
    {
#pragma warning disable CA2201
        throw new Exception("");
#pragma warning restore CA2201
    }
}
