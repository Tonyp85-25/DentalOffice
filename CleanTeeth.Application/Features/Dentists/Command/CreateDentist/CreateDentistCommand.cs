using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Services;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Utilities;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using FluentValidation;

namespace CleanTeeth.Application.Features.Dentists.Command.CreateDentist;

public class CreateDentistCommand:IRequest<Guid>
{
    public Guid Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public List<Guid> Offices { get; set; } = new();

}

public class CreateDentistCommandValidator : AbstractValidator<CreateDentistCommand>
{
    
    public CreateDentistCommandValidator(IDentistRepository dentistRepository
    )
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(30).MinimumLength(2);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(30).MinimumLength(2);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

    }
}


public class CreateDentistCommandHandler : IRequestHandler<CreateDentistCommand, Guid>
{
    private readonly IIdProvider _idProvider;
    private readonly IDentalOfficeRepository _dentalOfficeRepository;
    private readonly IDentistRepository _dentistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDentistCommandHandler(IIdProvider idProvider,IDentalOfficeRepository dentalOfficeRepository, IDentistRepository dentistRepository, IUnitOfWork unitOfWork)
    {
        _idProvider = idProvider;
        _dentalOfficeRepository = dentalOfficeRepository;
        _dentistRepository = dentistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateDentistCommand request)
    {
        List<DentalOffice> dentalOffices = new();
        if (request.Offices.Count > 0)
        {
            foreach (var id in request.Offices)
            {
               var office = await _dentalOfficeRepository.GetById(id);
               dentalOffices.Add(office);
            }
        }
        bool exists = await _dentistRepository.Exists(request.Email);
        if (exists)
        {
            throw new AlreadyExistsException();
        }

        var dentist = new Dentist
        {
            Id = _idProvider.GetId(),
            LastName = request.LastName,
            FirstName = request.FirstName,
            Email = new Email(request.Email),
        };

        foreach (var office in dentalOffices)
        {
            dentist.AddDentalOffice(office);
        }

        try
        {
            var result = await _dentistRepository.Add(dentist);
            await _unitOfWork.Commit();
            return result.Id;
        }
        catch (Exception e)
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}