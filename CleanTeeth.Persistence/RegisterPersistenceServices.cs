using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Services;
using CleanTeeth.Persistence.Repositories;
using CleanTeeth.Persistence.Services;
using CleanTeeth.Persistence.UnitsOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTeeth.Persistence;

public static class RegisterPersistenceServices
{
    private const int DefaultPort = 1433;
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,IConfiguration config)
    {
        string dbConnection = GetDBString(config);
        services.AddDbContext<CleanTeethDbContext>(options =>
            options.UseSqlServer(dbConnection));
        services.AddScoped<IDentalOfficeRepository, DentalOfficeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWorkEFCore>();
        services.AddScoped<IIdProvider, GuidProvider>();
        
        
        return services;
    }

    private static string GetDBString(IConfiguration config)
    {
        var strBuilder = new SqlConnectionStringBuilder(config.GetConnectionString("CleanTeethConnectionString"));
        if (string.IsNullOrEmpty(strBuilder.InitialCatalog))
        {
            strBuilder.DataSource = $"{config["DbHost"]},{config["DbPort"]??DefaultPort.ToString()}";
            strBuilder.InitialCatalog = config["DbName"];
            strBuilder.UserID = config["DbUser"];
            strBuilder.Password = config["DbPassword"];
        }
      
        return strBuilder.ConnectionString;
    } 
}