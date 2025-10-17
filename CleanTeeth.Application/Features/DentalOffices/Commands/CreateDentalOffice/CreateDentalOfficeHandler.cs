using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Services;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Utilities;
using CleanTeeth.Domain.Entities;
using FluentValidation;

namespace CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;

public class CreateDentalOfficeHandler : IRequestHandler<CreateDentalOfficeCommand,Guid>
{
    private readonly IDentalOfficeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdProvider _idProvider;


    public CreateDentalOfficeHandler(IDentalOfficeRepository repository, IUnitOfWork unitOfWork, IIdProvider idProvider)
    {
        this._repository = repository;
        this._unitOfWork = unitOfWork;
        _idProvider = idProvider;
    }

    public async Task<Guid> Handle(CreateDentalOfficeCommand command)
    {
        var dentalOffice = new DentalOffice(command.Name, _idProvider.GetId());
        try
        {
            var result = await _repository.Add(dentalOffice);
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