using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Services;
using CleanTeeth.Application.Utilities;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;

public class CreateDentalOfficeHandler : IRequestHandler<CreateDentalOfficeCommand,Guid>
{
    private readonly IDentalOfficeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdProvider _idProvider;


    public CreateDentalOfficeHandler(IDentalOfficeRepository repository, IUnitOfWork unitOfWork, IIdProvider idProvider)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _idProvider = idProvider;
    }

    public async Task<Guid> Handle(CreateDentalOfficeCommand request)
    {

        try
        {
            var address = Address.Create(request.Number, request.Street, request.Zipcode, request.City);
            var dentalOffice = DentalOffice.Create(request.Name,_idProvider.GetId(),address, request.OpeningDays) ;
            var result = await _repository.Add(dentalOffice);
            await _unitOfWork.Commit();
            return result.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback();
            throw;
        }

    }
}
