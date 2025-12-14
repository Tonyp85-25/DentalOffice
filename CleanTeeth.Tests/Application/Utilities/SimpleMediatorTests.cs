using System.Reflection;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Utilities;
using CleanTeeth.Tests.Infrastructure;
using FluentValidation;


namespace CleanTeeth.Tests.Application.Utilities;

[TestClass]
public class SimpleMediatorTests
{

    public class FalseRequest: IRequest<string>
    {
        public required string Name { get; set; }
    }

    public class FalseRequestValidator : AbstractValidator<FalseRequest>
    {
        public FalseRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The field {PropertyName} is required.");
        }
    }

    public class FalseRequestHandler : IRequestHandler<FalseRequest,string>
    {
        public async Task<string> Handle(FalseRequest request)
        {
            return await Task.FromResult($"{request.Name} handled");
        }
    }

    [TestMethod]
    public async Task Send_WithRegisteredHandler_HandleIsExecuted()
    {
        
        var request = new FalseRequest(){Name = "Name"};

        var serviceProvider = new FakeServiceProvider();
        
        serviceProvider.AddService<IRequestHandler<FalseRequest,string>,FalseRequestHandler>();
        
        var mediator = new SimpleMediator(serviceProvider);
        await mediator.Send(request);
      
        
    }

    [TestMethod]
    [ExpectedException(typeof(MediatorException))]
    public async Task Send_WithoutRegisteredHandler_Throws()
    {
        var request = new FalseRequest(){Name ="Name"};
        var serviceProvider = new FakeServiceProvider();

    
        var mediator = new SimpleMediator(serviceProvider);
        await mediator.Send(request);
    }

    [TestMethod]
    [ExpectedException(typeof(CustomValidationException))]
    public async Task Send_InvalidCommand_Throws()
    {
        var request = new FalseRequest() { Name = "" };
        var serviceProvider = new FakeServiceProvider();
        var validatorType = typeof(FalseRequestValidator).GetInterface("IValidator`1");
        serviceProvider.AddService<IValidator<FalseRequest>,FalseRequestValidator>();
        serviceProvider.AddService<IRequestHandler<FalseRequest,string>,FalseRequestHandler>();
        
        var mediator = new SimpleMediator(serviceProvider);
        await mediator.Send(request);
    }
}