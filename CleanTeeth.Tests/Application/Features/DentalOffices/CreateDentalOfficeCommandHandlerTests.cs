using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Services;
using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Tests.Infrastructure;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class CreateDentalOfficeCommandHandlerTests
{
    private StubDentalOfficeRepository _repository;
    private IUnitOfWork _unitOfWork;
    private CreateDentalOfficeHandler _handler;
    private IIdProvider _idProvider;

    [TestInitialize]
    public void Setup()
    {
        _repository = new StubDentalOfficeRepository();
        _unitOfWork = new StubUnitOfWork();
        _idProvider = new StubIdProvider();
        _handler = new CreateDentalOfficeHandler(_repository, _unitOfWork,_idProvider);
    }

    [TestMethod]
    public async Task Handle_ValidCommand_ReturnsDentalOfficeId()
    {
        var command = new CreateDentalOfficeCommand { Name = "Dental Office A" };
        
        var result = await _handler.Handle(command);
      
        Assert.AreEqual(Guid.Empty, result);
    }
    
    [TestMethod]
    public async Task Handle_WhenThereAreErrors_WeRollBack()
    {
        var dentalOffice = new DentalOffice("Dental Office A", Guid.Empty);
        await  _repository.Add(dentalOffice);
        _repository.MustThrows = true;
        var command = new CreateDentalOfficeCommand { Name = "Dental Office A" };
        await Assert.ThrowsExceptionAsync<Exception>(async () =>
        {
            await _handler.Handle(command);
        });
        Assert.AreEqual(1, _repository.Data.Count());
    }
}