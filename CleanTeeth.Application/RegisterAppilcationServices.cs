using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeeth.Application.Features.DentalOffices.Queries;
using CleanTeeth.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTeeth.Application;

public static class RegisterApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IMediator, SimpleMediator>();
        services.AddScoped<IRequestHandler<CreateDentalOfficeCommand, Guid>, CreateDentalOfficeHandler>();
        services
            .AddScoped<IRequestHandler<GetDentalOfficeDetailQuery, DentalOfficeDetailDTO>,
                GetDentalOfficeDetailQueryHandler>();
        return services;
    }
}