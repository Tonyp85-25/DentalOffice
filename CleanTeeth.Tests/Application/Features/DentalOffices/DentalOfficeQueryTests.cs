using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Tests.Infrastructure;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class DentalOfficeQueryTests
{
    private IDentalOfficeRepository _repository;
    private GetDentalOfficeDetailQueryHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _repository = new StubDentalOfficeRepository();
        _handler = new GetDentalOfficeDetailQueryHandler(_repository);
    }

    [TestMethod]
    public async Task Handle_DentalOfficeExists_ReturnsIt()
    {
        var dentalOffice = new DentalOffice("Dental Office A",Guid.Empty);
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
        // what??? 
        await _handler.Handle(query);
    }
    
    
    
    
}