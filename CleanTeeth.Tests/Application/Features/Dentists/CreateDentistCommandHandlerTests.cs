using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Services;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.Dentists.Command.CreateDentist;
using CleanTeeth.Application.Utilities;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using CleanTeeth.Tests.Infrastructure;
using FluentValidation;

namespace CleanTeeth.Tests.Application.Features.Dentists;

[TestClass]
public class CreateDentistCommandHandlerTests
{
    private IIdProvider _idProvider;
    private IDentalOfficeRepository _dentalOfficeRepository;
    private StubDentistRepository _dentistRepository;
    private IUnitOfWork _unitOfWork;
    private CreateDentistCommandHandler _handler;
   

    [TestInitialize]
    public void Setup()
    {
        _idProvider = new StubIdProvider();
        _dentistRepository = new StubDentistRepository();
        _dentalOfficeRepository = new StubDentalOfficeRepository();
        _unitOfWork = new StubUnitOfWork();
        _handler = new CreateDentistCommandHandler(_idProvider, _dentalOfficeRepository, _dentistRepository,
            _unitOfWork);
    }

    [TestMethod]
    public async Task HandleCreatesADentist()
    {
        var dentalOffices = new List<Guid> { Guid.Empty };
            
        
        var command = new CreateDentistCommand
        {
            Id=Guid.Empty,
            LastName="Tutu",
            FirstName="Toto",
            Email="toto@tutu.com",
            Offices= dentalOffices,
        };


        var dentistId = await _handler.Handle(command);
        Assert.AreEqual(Guid.Empty,dentistId);
  
    }
    
    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public async Task CreationFailsWhenDentistAlreadyExists()
    {
        
        var command = new CreateDentistCommand
        {
            Id=Guid.Empty,
            LastName="Tutu",
            FirstName="Toto",
            Email="toto@tutu.com",
        };
        var dentist = new Dentist
        {
            Id=Guid.NewGuid(),
            LastName="Tutu",
            FirstName="Toto",
            Email=new Email("toto@tutu.com"),
        };
        await _dentistRepository.Add(dentist);
        await _handler.Handle(command);
    }
    
    [TestMethod]
    public async Task WhenCreationFailsDatabaseIsTheSame()
    {
        var dentist = new Dentist
        {
            Id=Guid.NewGuid(),
            LastName="Tutu",
            FirstName="Toto",
            Email=new Email("toto@tutu.com"),
        };
        await _dentistRepository.Add(dentist);
        var command = new CreateDentistCommand
        {
            Id=Guid.Empty,
            LastName="Tutu",
            FirstName="Toto",
            Email="toto@tata.com",
        };
        _dentistRepository.MustThrow = true;
        try
        {
            await _handler.Handle(command);
        }
        catch { }
        
        var data = await _dentistRepository.GetAll();
        Assert.AreEqual(1,data.Count() );
    }
}