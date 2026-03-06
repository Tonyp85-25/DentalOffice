using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Domain.ValueObjects;
using CleanTeeth.Tests.Fixtures;
using CleanTeeth.Tests.Infrastructure;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class DentalOfficeSearchTest
{
    private IDentalOfficeRepository _repository;
    private GetDentalOfficeSearchQueryHandler _handler;
    private DentalOfficeBuilder _builder;


    [TestInitialize]
    public void Setup()
    {
        _repository = new StubDentalOfficeRepository();
        _handler = new GetDentalOfficeSearchQueryHandler(_repository);
        _builder = new DentalOfficeBuilder();

    }
    
    [TestMethod]
    public async Task GetExisitingDentalOfficebyNameReturnResults()
    {

        var dentalOffice = _builder.WithName("Dental Office A").Build();
        var dentalOffice2 = _builder.WithName("Dental Office B").WithGuid(Guid.NewGuid()).Build();
        
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
        var dentalOffice = _builder.WithName("Dental Office A").WithAddress("3;street;11112;city").Build();
        var dentalOffice2 = _builder.WithName("Dental Office B").WithAddress("3;street2;11111;city").Build();
        
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
        var dentalOffice = _builder.WithName("Dental Office A").WithAddress("3;street;11112;city").Build();
     
        var dentalOffice2 = _builder.WithName("Dental Office B").WithAddress("3;street2;11111;city2").Build();
           
        
        await _repository.Add(dentalOffice);
        await _repository.Add(dentalOffice2);

        var query = new GetDentalOfficeSearchQuery { City ="city2" };

        var result = await _handler.Handle(query);
        
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(true, result.Exists((e =>e.Name =="Dental Office B")));
    }
    

}