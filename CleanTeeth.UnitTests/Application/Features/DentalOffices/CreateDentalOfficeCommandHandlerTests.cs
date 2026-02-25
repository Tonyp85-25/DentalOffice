using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Services;
using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using CleanTeeth.Tests.Infrastructure;

namespace CleanTeeth.Tests.Application.Features.DentalOffices;

public class CreateDentalOfficeCommandBuilder
{
    private CreateDentalOfficeCommand _command;

    public CreateDentalOfficeCommandBuilder()
    {
        _command= new CreateDentalOfficeCommand
        {
            City = "city",
            Street = "",
            Name = "",
            Number = string.Empty,
            Zipcode = "11111"
        };
    }

    public CreateDentalOfficeCommandBuilder WithName(string name)
    {
        _command.Name = name;
        return this;
    }

    public CreateDentalOfficeCommand Build()
    {
        return _command;
    }
}
[TestClass]
public class CreateDentalOfficeCommandHandlerTests
{
    private StubDentalOfficeRepository _repository;
    private IUnitOfWork _unitOfWork;
    private CreateDentalOfficeHandler _handler;
    private IIdProvider _idProvider;
    private CreateDentalOfficeCommandBuilder _builder;

    [TestInitialize]
    public void Setup()
    {
        _repository = new StubDentalOfficeRepository();
        _unitOfWork = new StubUnitOfWork();
        _idProvider = new StubIdProvider();
        _handler = new CreateDentalOfficeHandler(_repository, _unitOfWork,_idProvider);
        _builder = new CreateDentalOfficeCommandBuilder();
    }

    [TestMethod]
    public async Task Handle_ValidCommand_ReturnsDentalOfficeId()
    {
        var command = _builder.WithName("Dental Office A").Build();
        
        var result = await _handler.Handle(command);
      
        Assert.AreEqual(Guid.Empty, result);
    }
    
    [TestMethod]
    public async Task Handle_WhenThereAreErrors_WeRollBack()
    {
        var address = Address.Create("", "street", "11111", "city");
        var dentalOffice = DentalOffice.Create("Dental Office A", Guid.Empty, address);
        await  _repository.Add(dentalOffice);
        _repository.MustThrows = true;
        var command = _builder.WithName("Dental Office B" ).Build();
        try
        {
            await _handler.Handle(command);
        }
        catch (Exception e)
        {
        }

        var dentalOffices = await _repository.GetAll();
        Assert.AreEqual(1, dentalOffices.Count());
    
        
    }
}