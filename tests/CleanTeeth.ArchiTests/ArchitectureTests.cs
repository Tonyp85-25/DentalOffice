using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnitV3;
using CleanTeeth.API.Controllers;
using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeeth.Application.Utilities;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Persistence;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CleanTeeth.ArchiTests;

public class ArchitectureTests
{
    private static readonly Architecture Architecture =
        new ArchLoader().LoadAssemblies(typeof(DentalOfficeController).Assembly,
            typeof(CreateDentalOfficeCommand).Assembly,typeof(DentalOffice).Assembly, typeof(RegisterPersistenceServices).Assembly).Build();

    private readonly IObjectProvider<IType> _apiLayer =
        Types().That().ResideInAssembly(typeof(DentalOfficeController).Assembly).As("Api Layer");

    private readonly IObjectProvider<IType> _applicationLayer =
        Types().That().ResideInAssembly(typeof(CreateDentalOfficeCommand).Assembly).As("Application Layer");

    private readonly IObjectProvider<IType> _domainLayer =
        Types().That().ResideInAssembly(typeof(DentalOffice).Assembly).As("Domain Layer");

    private readonly IObjectProvider<IType> _infrastructureLayer =
        Types().That().ResideInAssembly(typeof(RegisterPersistenceServices).Assembly).As("Infrastructure Layer");

    [Fact]
    public void DomainTypesShouldNotDependOnOthersLayers()
    {
        IArchRule rule = Types().That().Are(_domainLayer).Should().NotDependOnAny(_applicationLayer)
            .Because("It does not respect clean architecture principles")
            ;
        IArchRule rule2 = Types().That().Are(_domainLayer).Should().NotDependOnAny(_infrastructureLayer)
            .Because("It does not respect clean architecture principles")
            ;

        IArchRule rule3 = Types().That().Are(_domainLayer).Should().NotDependOnAny(_apiLayer)
            .Because("It does not respect clean architecture principles")
            ;

        rule.And(rule2).And(rule3).Check(Architecture);
    }

    [Fact]
    public void ApplicationTypesShouldDependOnlyFromDomain()
    {

        IArchRule rule = Types().That().Are(_applicationLayer).Should().NotDependOnAny(_apiLayer)
            .Because("It does not respect clean architecture principles");
        IArchRule rule2 = Types().That().Are(_applicationLayer).Should().NotDependOnAny(_infrastructureLayer)
            .Because("It does not respect clean architecture principles")
            ;
        rule.And(rule2).Check(Architecture);
    }

    [Fact]
    public void InfrastructureTypesShouldOnlyDependFromCore()
    {

        IArchRule rule = Types().That().Are(_infrastructureLayer).Should().NotDependOnAny(_apiLayer)
            .Because("It does not respect clean architecture principles");

        rule.Check(Architecture);
    }

    [Fact]
    public void RequestAndHandlersShouldBeDefinedInApplication()
    {
        IArchRule rule = Classes().That().ImplementInterface(typeof(IRequestHandler<,>)).Should().ResideInNamespaceMatching("CleanTeeth.Application.Features")
            .Because("It is their places");
        IArchRule rule2 = Classes().That().ImplementInterface(typeof(IRequest<>)).Should().ResideInNamespaceMatching("CleanTeeth.Application.Features")
            .Because("It is their places");

        rule.And(rule2).Check(Architecture);
    }
}
