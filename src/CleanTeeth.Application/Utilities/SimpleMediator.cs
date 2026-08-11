using CleanTeeth.Application.Exceptions;
using FluentValidation;
using FluentValidation.Results;

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
        await ApplyValidations(request);
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new MediatorException($"Handler was not found for {request.GetType().Name}");
        }

        var method = handlerType.GetMethod("Handle")!; // Handler is null checked above
        var response = method.Invoke(handler, new object[] { request });
        if (response is null)
        {
            throw new MediatorException($"Response was null for {request.GetType().Name}");
        }
        return await (Task<TResponse>)response;

    }

    public async Task Send(IRequest request)
    {
       await  ApplyValidations(request);
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());

        var handler = _serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new MediatorException($"Handler was not found for {request.GetType().Name}");
        }

        var method = handlerType.GetMethod("Handle")!;
        var response = method.Invoke(handler, new object[] { request });
        if (response is null)
        {
            throw new MediatorException($"Response was null for {request.GetType().Name}");
        }
        await (Task)response;

    }

    private async Task ApplyValidations(object request)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
        var validator = _serviceProvider.GetService(validatorType);
        if (validator is not null)
        {
            var validateMethod = validatorType.GetMethod("ValidateAsync")!; // Validator is null checked above

            var taskToValidate =
                (Task)validateMethod.Invoke(validator, new object[] { request, CancellationToken.None })!;
            await taskToValidate;
            var resultProperty = taskToValidate.GetType().GetProperty("Result");
            if (resultProperty is null)
            {
                throw new MediatorException($"Result property was not found for {request.GetType().Name}");
            }

            var result = resultProperty.GetValue(taskToValidate);
            if (result is null)
            {
                throw new MediatorException($"Result was null for {request.GetType().Name}");
            }
            var validationResult = (ValidationResult)result;

            if (!validationResult.IsValid)
            {
                throw new CustomValidationException(validationResult);
            }
        }
    }
}
