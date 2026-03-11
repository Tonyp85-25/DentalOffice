using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using CleanTeeth.Fixtures.Entities;
using CleanTeeth.Tests.Infrastructure;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class DentalOfficeQueryTests
{
    private IDentalOfficeRepository _repository;
    private GetDentalOfficeDetailQueryHandler _handler;
    private DentalOfficeBuilder _builder;

    [TestInitialize]
    public void Setup()
    {
        _repository = new StubDentalOfficeRepository();
        _handler = new GetDentalOfficeDetailQueryHandler(_repository);
        _builder = new DentalOfficeBuilder();
    }

    [TestMethod]
    public async Task Handle_DentalOfficeExists_ReturnsIt()
    {
        var dentalOffice = _builder.WithName("Dental Office A").Build();
        var id = dentalOffice.Id;
        await _repository.Add(dentalOffice);
        var query = new GetDentalOfficeDetailQuery { Id = id };
        
        var result = await _handler.Handle(query);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(id,result.Id);
        Assert.AreEqual("Dental Office A", result.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task Handle_DentalDoesNotExistsThrow()
    {
        var id = Guid.Empty;
        var query = new GetDentalOfficeDetailQuery { Id = id };
     
        await _handler.Handle(query);
    }
    
    
    
    
}