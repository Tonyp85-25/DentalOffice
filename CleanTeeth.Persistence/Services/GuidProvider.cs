using CleanTeeth.Application.Contracts.Services;

namespace CleanTeeth.Persistence.Services;

public class GuidProvider:IIdProvider
{
    public Guid GetId()
    {
        return Guid.NewGuid();
    }
}