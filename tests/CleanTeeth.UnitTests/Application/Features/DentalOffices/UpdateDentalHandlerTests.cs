using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Services;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.DentalOffices.Commands;
using CleanTeeth.Fixtures.Entities;
using CleanTeeth.Tests.Infrastructure;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

[TestClass]
public class UpdateDentalHandlerTests
{
    private StubDentalOfficeRepository _repository;
    private IUnitOfWork _unitOfWork;
    private UpdateDentalOfficeHandler _handler;
    
    [TestInitialize]
    public void Setup()
    {
        _repository = new StubDentalOfficeRepository();
        _unitOfWork = new StubUnitOfWork();
        _handler = new UpdateDentalOfficeHandler(_repository, _unitOfWork);

    }
    
    [TestMethod]
    public async Task Handle_ValidCommand_Returns_Updated_DentalOffice()
    {
        //arrange
        var dentalOffice = new DentalOfficeBuilder().Build();
        _repository.Add(dentalOffice);

        //act
        var command = new UpdateDentalOfficeCommand
        {
            Id = dentalOffice.Id,
            Name = "Updated Dental Office",
            Number = "4",
            Street = "rue du quai",
            Zipcode = "55555",
            City = "UnevilleUpdated",
            AdditionalInfos = null,
            OpeningDays = 48
        };
        
        var result = await _handler.Handle(command);
        
        //assert
        Assert.AreEqual(dentalOffice.Id, result.Id);
        Assert.AreEqual("Updated Dental Office", result.Name);
        Assert.AreEqual("4", result.Number);
        Assert.AreEqual("rue du quai", result.Street);
        Assert.AreEqual("55555", result.Zipcode);
        Assert.AreEqual("UnevilleUpdated", result.City);
        Assert.AreEqual(48, result.OpeningDays);

    }
    
    [TestMethod]
    public async Task Invalid_Command_DoesNotUpdate_DentalOffice()
    {
        var dentalOffice = new DentalOfficeBuilder().Build();
        _repository.Add(dentalOffice);
        
        //act
        var command = new UpdateDentalOfficeCommand
        {
            Id = dentalOffice.Id,
            Number = "4",
            Street = "r",
            Zipcode = "55",
            City = "UnevilleUpdated",
            AdditionalInfos = null,
            OpeningDays = 48
        };
        try
        {
            await _handler.Handle(command);
        }
        catch (Exception e)
        {
        }
        var dentalOfficeAfter = await _repository.GetById(dentalOffice.Id);
        Assert.AreEqual(dentalOffice, dentalOfficeAfter);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task IncorectId_Throws_Exception()
    {
        var dentalOffice = new DentalOfficeBuilder().Build();
        _repository.Add(dentalOffice);
        
        var command = new UpdateDentalOfficeCommand
        {
            Id = Guid.NewGuid(),
            Number = "4",
            Street = "r",
            Zipcode = "55",
            City = "UnevilleUpdated",
            AdditionalInfos = null,
            OpeningDays = 48
        };
       
            await _handler.Handle(command);
    }
}