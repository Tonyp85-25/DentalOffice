using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Utilities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace CleanTeeth.Tests.Application.Utilities;

[TestClass]
public class SimpleMediatorTests
{
    public class FalseRequest: IRequest<string>
    {
    }

    [TestMethod]
    public async Task Send_WithRegisteredHandler_HandleIsExecuted()
    {
        //TODO replace mocks with stubs and expect a predictable result
        var request = new FalseRequest();

        var handlerMock = Substitute.For<IRequestHandler<FalseRequest, string>>();

        var serviceProvider = Substitute.For<IServiceProvider>();

        serviceProvider.GetService(typeof(IRequestHandler<FalseRequest, string>))
            .Returns(handlerMock);
            
        var mediator = new SimpleMediator(serviceProvider);
        var result = await mediator.Send(request);
        await handlerMock.Received(1).Handle(request);
        
    }

    [TestMethod]
    [ExpectedException(typeof(MediatorException))]
    public async Task Send_WithoutRegisteredHandler_Throws()
    {
        var request = new FalseRequest();
        var serviceProvider = Substitute.For<IServiceProvider>();

        serviceProvider.GetService(typeof(IRequestHandler<FalseRequest, string>))
            .ReturnsNull();
        var mediator = new SimpleMediator(serviceProvider);
        var result = await mediator.Send(request);
    }
    
}