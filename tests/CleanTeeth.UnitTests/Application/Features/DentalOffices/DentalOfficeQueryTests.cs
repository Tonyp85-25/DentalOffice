using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Fixtures.Entities;
using CleanTeeth.Tests.Infrastructure;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class DentalOfficeQueryTests
{
    private StubDentalOfficeRepository _repository;
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
    public async Task HandleDentalOfficeExistsReturnsIt()
    {
        var dentalOffice = _builder.WithName("Dental Office A").Build();
        var id = dentalOffice.Id;
        await _repository.Add(dentalOffice);
        var query = new GetDentalOfficeDetailQuery { Id = id };

        var result = await _handler.Handle(query);

        Assert.IsNotNull(result);
        Assert.AreEqual(id,result.Id);
        Assert.AreEqual("Dental Office A", result.Name, StringComparer.Ordinal);
    }

    [TestMethod]
    public async Task HandleDentalDoesNotExistsThrow()
    {

        await Assert.ThrowsExceptionAsync<NotFoundException>(async() =>
        {
            var id = Guid.Empty;
            var query = new GetDentalOfficeDetailQuery { Id = id };

            await _handler.Handle(query);
        });
    }




}
