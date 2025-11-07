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
        services.Scan(scan => scan.FromAssembliesOf(typeof(RegisterApplicationServices))
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
        return services;
    }
}