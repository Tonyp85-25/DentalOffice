using CleanTeeth.Application.Contracts.Persistence;

namespace CleanTeeth.Persistence.UnitsOfWork;

public class UnitOfWorkEFCore:IUnitOfWork
{
    private readonly CleanTeethDbContext _context;

    public UnitOfWorkEFCore(CleanTeethDbContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public Task Rollback()
    {
        return Task.CompletedTask;
    }
}