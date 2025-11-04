using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Utilities;

namespace CleanTeeth.Application.Features.DentalOffices.Queries;


public class GetDentalOfficeListQueryHandler: IRequestHandler<GetDentalOfficeListQuery,List<DentalOfficeListDTO>>
{
    private readonly IDentalOfficeRepository _repository;

    public GetDentalOfficeListQueryHandler(IDentalOfficeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DentalOfficeListDTO>> Handle(GetDentalOfficeListQuery request)
    {
        var list = await _repository.GetAll();
        var result = list.Select(d=>  new DentalOfficeListDTO
        {
            Id = d.Id,
            Name = d.Name
        });
        return  result.ToList();
    }
}

public class DentalOfficeListDTO
{
    public Guid Id { get; set; }
    public String Name { get; set; }
}