using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Tests.Infrastructure;

public class StubDentalOfficeRepository: IDentalOfficeRepository
{
    public bool MustThrows { get; set; } = false;
    public List<DentalOffice> Data { get; }= new();
    public Task<DentalOffice?> GetById(Guid id)
    {
        return Task.FromResult(Data.Find(d => d.Id == id)) ;
    }

    public Task<IEnumerable<DentalOffice>> GetAll()
    {
        return Task.FromResult<IEnumerable<DentalOffice>>(Data);
    }

    public Task<DentalOffice> Add(DentalOffice entity)
    {
        if (MustThrows)
        {
            Throws();
        }
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

    public void Throws()
    {
        throw new Exception();
    }
}