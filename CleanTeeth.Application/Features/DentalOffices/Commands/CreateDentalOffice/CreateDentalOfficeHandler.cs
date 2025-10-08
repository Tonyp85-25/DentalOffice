using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;

public class CreateDentalOfficeHandler
{
    private readonly IDentalOfficeRepository _repository;
    public CreateDentalOfficeHandler(IDentalOfficeRepository repository)
    {
        this._repository = repository;
    }

    public async Task<Guid> Handle(CreateDentalOfficeCommand command)
    {
        var dentalOffice = new DentalOffice(command.Name);
        var result = await _repository.Add(dentalOffice);
        return result.Id;
    }
}

