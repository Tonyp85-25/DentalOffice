using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Services;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.BackOffice.Dentists.Command.CreateDentist;
using CleanTeeth.Application.Utilities;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using CleanTeeth.Tests.Infrastructure;
using FluentValidation;

#pragma warning disable S3261
namespace CleanTeeth.Tests.Application.Features.Dentists;
#pragma warning restore S3261

[TestClass]
public class CreateDentistCommandHandlerTests

 {

    private IIdProvider _idProvider;
    private StubDentalOfficeRepository _dentalOfficeRepository;
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
        var officeId = Guid.NewGuid();
        var dentalOffices = new List<Guid> { officeId};
        var dentalOffice = DentalOffice.Create("test", officeId,
            Address.Create(null, "street", "12345", "City", "State"), 30);
        dentalOffice.Publish();
        _dentalOfficeRepository.Add(dentalOffice);



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


        await Assert.ThrowsExceptionAsync<AlreadyExistsException>(() => _handler.Handle(command));


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
        catch
        {
            var data = await _dentistRepository.GetAll();
            Assert.AreEqual(1,data.Count() );

        }


    }
}
