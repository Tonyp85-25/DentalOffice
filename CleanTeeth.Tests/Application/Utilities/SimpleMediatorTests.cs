using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Utilities;
using FluentValidation;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

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

    [TestMethod]
    public async Task Send_WithRegisteredHandler_HandleIsExecuted()
    {
        //TODO replace mocks with stubs and expect a predictable result
        var request = new FalseRequest(){Name = "Name"};

        var handlerMock = Substitute.For<IRequestHandler<FalseRequest, string>>();

        var serviceProvider = Substitute.For<IServiceProvider>();

        serviceProvider.GetService(typeof(IRequestHandler<FalseRequest, string>))
            .Returns(handlerMock);
            
        var mediator = new SimpleMediator(serviceProvider);
        await mediator.Send(request);
        await handlerMock.Received(1).Handle(request);
        
    }

    [TestMethod]
    [ExpectedException(typeof(MediatorException))]
    public async Task Send_WithoutRegisteredHandler_Throws()
    {
        var request = new FalseRequest(){Name ="Name"};
        var serviceProvider = Substitute.For<IServiceProvider>();

        serviceProvider.GetService(typeof(IRequestHandler<FalseRequest, string>))
            .ReturnsNull();
        var mediator = new SimpleMediator(serviceProvider);
        await mediator.Send(request);
    }

    [TestMethod]
    [ExpectedException(typeof(CustomValidationException))]
    public async Task Send_InvalidCommand_Throws()
    {
        var request = new FalseRequest() { Name = "" };
        var serviceProvider = Substitute.For<IServiceProvider>();
        var validator = new FalseRequestValidator();

        serviceProvider.GetService(typeof(IValidator<FalseRequest>)).Returns(validator);
        var mediator = new SimpleMediator(serviceProvider);
        await mediator.Send(request);
    }
}