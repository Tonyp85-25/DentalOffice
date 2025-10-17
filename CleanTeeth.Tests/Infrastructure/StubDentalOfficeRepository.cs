using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Tests.Infrastructure;

public class StubDentalOfficeRepository: IDentalOfficeRepository
{
    private List<DentalOffice> Data = new();
    public Task<DentalOffice?> GetById(Guid id)
    {
        return Task.FromResult(Data.Find(d => d.Id == id)) ;
    }

    public Task<IEnumerable<DentalOffice>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<DentalOffice> Add(DentalOffice entity)
    {
        Data.Add(entity);
        return Task.FromResult(entity);
    }

    public Task Update(DentalOffice entity)
    {
        throw new NotImplementedException();
    }

    public Task Delete(DentalOffice entity)
    {
        throw new NotImplementedException();
    }
}