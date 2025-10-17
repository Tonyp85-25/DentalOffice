using CleanTeeth.Application.Contracts.Services;

namespace CleanTeeth.Tests.Infrastructure;

public class StubIdProvider:IIdProvider
{
    public Guid GetId()
    {
        return Guid.Empty;
    }
}