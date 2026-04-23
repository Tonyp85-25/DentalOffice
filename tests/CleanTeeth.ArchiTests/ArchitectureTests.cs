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
    
    private readonly IObjectProvider<IType> ApiLayer =
        Types().That().ResideInAssembly(typeof(DentalOfficeController).Assembly).As("Api Layer");
    
    private readonly IObjectProvider<IType> ApplicationLayer =
        Types().That().ResideInAssembly(typeof(CreateDentalOfficeCommand).Assembly).As("Application Layer");
    
    private readonly IObjectProvider<IType> DomainLayer =
        Types().That().ResideInAssembly(typeof(DentalOffice).Assembly).As("Domain Layer");
    
    private readonly IObjectProvider<IType> InfrastructureLayer =
        Types().That().ResideInAssembly(typeof(RegisterPersistenceServices).Assembly).As("Infrastructure Layer");

    [Fact]
    public void DomainTypesShouldNotDependOnOthersLayers()
    {
        IArchRule rule = Types().That().Are(DomainLayer).Should().NotDependOnAny(ApplicationLayer)
            .Because("It does not respect clean architecture principles")
            ;
        IArchRule rule2 = Types().That().Are(DomainLayer).Should().NotDependOnAny(InfrastructureLayer)
            .Because("It does not respect clean architecture principles")
            ;
            
        IArchRule rule3 = Types().That().Are(DomainLayer).Should().NotDependOnAny(ApiLayer)
            .Because("It does not respect clean architecture principles")
            ;

        rule.And(rule2).And(rule3).Check(Architecture);
    }

    [Fact]
    public void ApplicationTypesShouldDependOnlyFromDomain()
    {
        
        IArchRule rule = Types().That().Are(ApplicationLayer).Should().NotDependOnAny(ApiLayer)
            .Because("It does not respect clean architecture principles");
        IArchRule rule2 = Types().That().Are(ApplicationLayer).Should().NotDependOnAny(InfrastructureLayer)
            .Because("It does not respect clean architecture principles")
            ;
        rule.And(rule2).Check(Architecture);
    }
    
    [Fact]
    public void InfrastructureTypesShouldOnlyDependFromCore()
    {
        
        IArchRule rule = Types().That().Are(InfrastructureLayer).Should().NotDependOnAny(ApiLayer)
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