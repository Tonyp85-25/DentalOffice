using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Fixtures.Entities;
using CleanTeeth.Tests.Infrastructure;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class DentalOfficesListTest
{
    private StubDentalOfficeRepository _repository;
    private GetDentalOfficeListQueryHandler _handler;
    private DentalOfficeBuilder _builder;

    [TestInitialize]
    public void Setup()
    {
        _repository = new StubDentalOfficeRepository();
        _handler = new GetDentalOfficeListQueryHandler(_repository);
        _builder = new DentalOfficeBuilder();
    }

    [TestMethod]
    public async Task HandleDentalOfficeListReturnsIt()
    {
        var dentalOffice = _builder.Build();
        var dentalOffice2 = _builder.WithName("Dental office B").Build();
        await _repository.Add(dentalOffice);
        await _repository.Add(dentalOffice2);
        var query = new GetDentalOfficeListQuery();

        List<DentalOfficeListDto> result = await _handler.Handle(query);

        Assert.AreEqual(2, result.Count);
        foreach (var office in result)
        {
            Assert.AreEqual(Guid.Empty, office.Id);
        }
    }

    [TestMethod]
    public async Task HandleEmptyDentalOfficeListReturnsEmptyList()
    {
        var query = new GetDentalOfficeListQuery();

        List<DentalOfficeListDto> result = await _handler.Handle(query);

        Assert.AreEqual(0, result.Count);

    }
}
