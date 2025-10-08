using CleanTeeth.Application.Exceptions;

namespace CleanTeeth.Application.Utilities;

public class SimpleMediator: IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public SimpleMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new MediatorException($"Handler was not found for{request.GetType().Name}");
        }

        var method = handlerType.GetMethod("Handle");
        return await (Task<TResponse>)method.Invoke(handler, new object[] { request });

    }
}