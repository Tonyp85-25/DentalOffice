using CleanTeeth.Domain.Exceptions;

namespace CleanTeeth.Domain.ValueObjects;

public record TimeInterval()
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public TimeInterval(DateTime start, DateTime end) : this()
    {
        if (start > end)
        {
            throw new BusinessRuleException("The start time cannot be after start time");
        }
        Start = start;
        End = end;
    }
}