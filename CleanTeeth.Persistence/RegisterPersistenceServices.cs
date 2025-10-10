using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Persistence.Repositories;
using CleanTeeth.Persistence.UnitsOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTeeth.Persistence;

public static class RegisterPersistenceServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddDbContext<CleanTeethDbContext>(options =>
            options.UseSqlServer("name=CleanTeethConnectionString"));
        services.AddScoped<IDentalOfficeRepository, DentalOfficeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWorkEFCore>();
        
        
        return services;
    }
}