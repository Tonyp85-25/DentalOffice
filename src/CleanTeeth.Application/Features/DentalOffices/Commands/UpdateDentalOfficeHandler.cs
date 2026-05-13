using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Application.Utilities;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using FluentValidation;

namespace CleanTeeth.Application.Features.DentalOffices.Commands;


public class UpdateDentalOfficeCommand: IRequest<DentalOfficeDetailDTO>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? Street { get; set; }
    public string? Zipcode { get; set; }
    public string? City { get; set; }
   
    public int? OpeningDays { get; set; }
    public string ?AdditionalInfos { get; set; }
}

public class UpdateDentalOfficeCommandValidator: AbstractValidator<UpdateDentalOfficeCommand>{

    public UpdateDentalOfficeCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty();
        RuleFor(u => u.Name).MaximumLength(50).When(u => u.Name != null);
        RuleFor(u => u.Number).MaximumLength(5).When(u => u.Number != null);
        RuleFor(p => p.Street).MaximumLength(50).MinimumLength(5).When(p => p.Street != null);
        RuleFor(u => u.Zipcode).Matches("/[0-9]{5}/gm").When(u => u.Zipcode != null);
        RuleFor(u => u.City).MaximumLength(50).MinimumLength(2).When(u => u.City != null);
        RuleFor(u => u.OpeningDays).Must(p => p%2 ==0 ).When(u => u.OpeningDays != null);
    }
}

public class UpdateDentalOfficeHandler: IRequestHandler<UpdateDentalOfficeCommand, DentalOfficeDetailDTO>
{
    private readonly IDentalOfficeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDentalOfficeHandler(IDentalOfficeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DentalOfficeDetailDTO> Handle(UpdateDentalOfficeCommand request)
    {
        var dentalOffice = await _repository.GetById(request.Id);
        if (dentalOffice is null)
        {
            throw new NotFoundException();
        }

        
        try
      
        {    
            dentalOffice.UpdateName(request.Name).UpdateAddress(request.Number, request.Street, request.Zipcode, request.City, request.AdditionalInfos).UpdateOpeningDays(request.OpeningDays);
            await _repository.Update(dentalOffice);
            await _unitOfWork.Commit();
            return dentalOffice.ToDTO();
           
        }
        catch (Exception e)
        {
           await _unitOfWork.Rollback();
            throw;
        }
    }
}