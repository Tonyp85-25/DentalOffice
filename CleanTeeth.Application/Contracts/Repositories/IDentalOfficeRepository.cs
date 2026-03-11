using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Contracts.Repositories;

public record DentalOfficeCriteria(string? Name, string? Zipcode, string? City, int? Days);
public interface IDentalOfficeRepository :IRepository<DentalOffice>

{
    public Task<IEnumerable<DentalOffice>> GetAllBy(DentalOfficeCriteria criteria);


}