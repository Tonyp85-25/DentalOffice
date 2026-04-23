using CleanTeeth.Application.Contracts.Persistence;

namespace CleanTeeth.Tests.Infrastructure;

public class StubUnitOfWork :IUnitOfWork
{
    public Task Commit()
    {
        return Task.CompletedTask;
    }

    public Task Rollback()
    {
        return Task.CompletedTask;
    }
}