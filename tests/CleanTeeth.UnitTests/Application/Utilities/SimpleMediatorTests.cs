using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Utilities;
using CleanTeeth.Tests.Infrastructure;
using FluentValidation;


namespace CleanTeeth.Tests.Application.Utilities;

[TestClass]
public class SimpleMediatorTests
{
    private sealed class FalseRequest: IRequest<string>
    {
        public required string Name { get; set; }
    }

    private sealed class FalseRequestValidator : AbstractValidator<FalseRequest>
    {
#pragma warning disable S1144
        public FalseRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The field {PropertyName} is required.");
        }
#pragma warning restore S1144
    }

    private class FalseRequestHandler : IRequestHandler<FalseRequest,string>
    {
        public async Task<string> Handle(FalseRequest request)
        {
            return await Task.FromResult($"{request.Name} handled");
        }
    }

    [TestMethod]
    public async Task SendWithRegisteredHandlerHandleIsExecuted()
    {

        var request = new FalseRequest(){Name = "Name"};

        var serviceProvider = new FakeServiceProvider();

        serviceProvider.AddService<IRequestHandler<FalseRequest,string>,FalseRequestHandler>();

        var mediator = new SimpleMediator(serviceProvider);
        var result =await mediator.Send(request);
        Assert.AreEqual("Name handled", result, StringComparer.Ordinal);


    }

    [TestMethod]
    public async Task SendWithoutRegisteredHandlerThrows()
    {
        var request = new FalseRequest(){Name ="Name"};
        var serviceProvider = new FakeServiceProvider();


        var mediator = new SimpleMediator(serviceProvider);
        await Assert.ThrowsExceptionAsync<MediatorException>(async () => await mediator.Send(request));

    }

    [TestMethod]
    public async Task SendInvalidCommandThrows()
    {
        var request = new FalseRequest() { Name = "" };
        var serviceProvider = new FakeServiceProvider();

        serviceProvider.AddService<IValidator<FalseRequest>,FalseRequestValidator>();
        serviceProvider.AddService<IRequestHandler<FalseRequest,string>,FalseRequestHandler>();

        var mediator = new SimpleMediator(serviceProvider);
        await Assert.ThrowsExceptionAsync<CustomValidationException>(async () =>
        {
            await mediator.Send(request);
        });

    }
}
