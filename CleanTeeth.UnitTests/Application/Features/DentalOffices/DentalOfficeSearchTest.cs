using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using CleanTeeth.Tests.Infrastructure;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class DentalOfficeSearchTest
{
    private IDentalOfficeRepository _repository;
    private GetDentalOfficeSearchQueryHandler _handler;


    [TestInitialize]
    public void Setup()
    {
        _repository = new StubDentalOfficeRepository();
        _handler = new GetDentalOfficeSearchQueryHandler(_repository);

    }
    
    [TestMethod]
    public async Task GetExisitingDentalOfficebyNameReturnResults()
    {
        var address = Address.Create("", "street", "11111", "city");
        var dentalOffice = DentalOffice.Create("Dental Office A",Guid.Empty, address);
        var dentalOffice2 = DentalOffice.Create("Dental Office B",Guid.NewGuid(), address);
        
        await _repository.Add(dentalOffice);
        await _repository.Add(dentalOffice2);

        var query = new GetDentalOfficeSearchQuery { Name = "A" };

        var result = await _handler.Handle(query);
        
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(true, result.Exists((e =>e.Name =="Dental Office A")));
    }
    
    [TestMethod]
    public async Task GetExisitingDentalOfficebyZipcodeReturnResults()
    {
        var address = Address.Create("", "street", "11112", "city");
        var address2 = Address.Create("", "street2", "11111", "city");
        var dentalOffice = DentalOffice.Create("Dental Office A",Guid.Empty, address);
        var dentalOffice2 = DentalOffice.Create("Dental Office B",Guid.NewGuid(), address2);
        
        await _repository.Add(dentalOffice);
        await _repository.Add(dentalOffice2);

        var query = new GetDentalOfficeSearchQuery { Zipcode="11112" };

        var result = await _handler.Handle(query);
        
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(true, result.Exists((e =>e.Name =="Dental Office A")));
    }
    
    [TestMethod]
    public async Task GetExistingDentalOfficebyCityReturnResults()
    {
        var address = Address.Create("", "street", "11112", "city");
        var address2 = Address.Create("", "street2", "11111", "city2");
        var dentalOffice = DentalOffice.Create("Dental Office A",Guid.Empty, address);
        var dentalOffice2 = DentalOffice.Create("Dental Office B",Guid.NewGuid(), address2);
        
        await _repository.Add(dentalOffice);
        await _repository.Add(dentalOffice2);

        var query = new GetDentalOfficeSearchQuery { City ="city2" };

        var result = await _handler.Handle(query);
        
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(true, result.Exists((e =>e.Name =="Dental Office B")));
    }
    

}