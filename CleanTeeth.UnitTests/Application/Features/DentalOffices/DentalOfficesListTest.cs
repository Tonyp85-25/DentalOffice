using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using CleanTeeth.Tests.Infrastructure;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class DentalOfficesListTest
{
    private IDentalOfficeRepository _repository;
    private GetDentalOfficeListQueryHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _repository = new StubDentalOfficeRepository();
        _handler = new GetDentalOfficeListQueryHandler(_repository);
    }

    [TestMethod]
    public async Task Handle_DentalOfficeList_ReturnsIt()
    {
        var address = Address.Create("", "street", "11111", "city");
        
        var dentalOffice = DentalOffice.Create("Dental Office A", Guid.Empty,address);
        var dentalOffice2 = DentalOffice.Create("Dental Office B", Guid.Empty, address);
        await _repository.Add(dentalOffice);
        await _repository.Add(dentalOffice2);
        var query = new GetDentalOfficeListQuery();

        List<DentalOfficeListDTO> result = await _handler.Handle(query);

        Assert.AreEqual(2, result.Count);
        foreach (var office in result)
        {
            Assert.AreEqual(Guid.Empty, office.Id);
        }
    }
    
    [TestMethod]
    public async Task Handle_EmptyDentalOfficeList_ReturnsEmptyList()
    {
        var query = new GetDentalOfficeListQuery();

        List<DentalOfficeListDTO> result = await _handler.Handle(query);

        Assert.AreEqual(0, result.Count);
       
    }
}