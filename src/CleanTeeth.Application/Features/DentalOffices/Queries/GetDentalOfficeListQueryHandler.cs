using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Utilities;

namespace CleanTeeth.Application.Features.DentalOffices.Queries;


public class GetDentalOfficeListQueryHandler: IRequestHandler<GetDentalOfficeListQuery,List<DentalOfficeListDto>>
{
    private readonly IDentalOfficeRepository _repository;

    public GetDentalOfficeListQueryHandler(IDentalOfficeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DentalOfficeListDto>> Handle(GetDentalOfficeListQuery request)
    {
        var list = await _repository.GetAll();
        var result = list.Select(d=>  new DentalOfficeListDto
        {
            Id = d.Id,
            Name = d.Name
        });
        return  result.ToList();
    }
}

public class DentalOfficeListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
