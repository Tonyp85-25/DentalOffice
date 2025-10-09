using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeeth.Domain.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class CreateDentalOfficeCommandHandlerTests
{
    private IDentalOfficeRepository _repository;
    private IUnitOfWork _unitOfWork;
    private CreateDentalOfficeHandler _handler;

    [TestInitialize]
    public void Setup()
    {   // seriously... I would prefer a stub instead
        _repository = Substitute.For<IDentalOfficeRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateDentalOfficeHandler(_repository, _unitOfWork);
    }

    [TestMethod]
    public async Task Handle_ValidCommand_ReturnsDentalOfficeId()
    {
        var command = new CreateDentalOfficeCommand { Name = "Dental Office A" };

        var dentalOffice = new DentalOffice("Dental Office A");

        _repository.Add(Arg.Any<DentalOffice>()).Returns(dentalOffice);
        var result = await _handler.Handle(command);
        await _repository.Received(1).Add(Arg.Any<DentalOffice>());
        await _unitOfWork.Received(1).Commit();
        Assert.AreEqual(dentalOffice.Id,result);
    }
    
    [TestMethod]
    public async Task Handle_WhenThereAreErrors_WeRollBack()
    {
        var command = new CreateDentalOfficeCommand { Name = "Dental Office A" };
        _repository.Add(Arg.Any<DentalOffice>()).Throws<Exception>();

        await Assert.ThrowsExceptionAsync<Exception>(async () =>
        {
            await _handler.Handle(command);
        });
        await _unitOfWork.Received(1).Rollback();
    }
}