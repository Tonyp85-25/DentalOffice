namespace CleanTeeth.Application.Utilities;

public interface IRequestHandler<TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request);
}

public interface IRequestHandler<in TRequest>
{
    Task Handle(TRequest request);
}
