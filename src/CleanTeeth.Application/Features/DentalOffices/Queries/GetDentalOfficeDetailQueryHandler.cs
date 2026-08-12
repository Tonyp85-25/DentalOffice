using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Utilities;

namespace CleanTeeth.Application.Features.DentalOffices.Queries;

public class GetDentalOfficeDetailQueryHandler :IRequestHandler<GetDentalOfficeDetailQuery,DentalOfficeDetailDto>
{
    private readonly IDentalOfficeRepository _repository;

    public GetDentalOfficeDetailQueryHandler(IDentalOfficeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DentalOfficeDetailDto> Handle(GetDentalOfficeDetailQuery request)
    {
        var dentalOffice = await _repository.GetById(request.Id) ?? throw new NotFoundException();
        return dentalOffice.ToDto();
    }
}
