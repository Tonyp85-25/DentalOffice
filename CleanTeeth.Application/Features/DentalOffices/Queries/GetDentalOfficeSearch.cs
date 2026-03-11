using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Utilities;

namespace CleanTeeth.Application.Features.DentalOffices.Queries;

public class GetDentalOfficeSearchQuery:IRequest<List<DentalOfficeListDTO>>
{
    public   string? Name { get; init; }
    public   string? Zipcode { get; init; }
    
    public   string? City { get; init; }
    
    public int? Days { get; init; }
    
}

public class GetDentalOfficeSearchQueryHandler :IRequestHandler<GetDentalOfficeSearchQuery,List<DentalOfficeListDTO>>
{
    private readonly IDentalOfficeRepository _repository;

    public GetDentalOfficeSearchQueryHandler( IDentalOfficeRepository repository)
    
    {
        _repository = repository;
    }

    public  async Task<List<DentalOfficeListDTO>> Handle(GetDentalOfficeSearchQuery request)
    {
        var criteria = new DentalOfficeCriteria(request.Name, request.Zipcode, request.City, request.Days);
        var list = await _repository.GetAllBy(criteria);
        var result = list.Select(d=>  new DentalOfficeListDTO
        {
            Id = d.Id,
            Name = d.Name
        });
        return  result.ToList();
    }
}