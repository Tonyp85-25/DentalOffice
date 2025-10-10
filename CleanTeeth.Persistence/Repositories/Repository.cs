using CleanTeeth.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T :class
{
    private readonly CleanTeethDbContext _context;

    public Repository(CleanTeethDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetById(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public Task<T> Add(T entity)
    {
        _context.Add(entity);
        return Task.FromResult(entity);
    }

    public Task Update(T entity)
    {

        _context.Update(entity);
        return Task.CompletedTask;
    }

    public Task Delete(T entity)
    {
        _context.Remove(entity);
        return Task.CompletedTask;
    }
}