namespace CleanTeeth.Application.Exceptions;

public class MediatorException:Exception
{
    public MediatorException(string message) : base(message)
    {
    }

    public MediatorException()
    {
    }

    public MediatorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
