namespace CleanTeeth.Application.Contracts.Repositories;

public interface IRepository<T> where T:class
{
    
    // disagree : not all of these methods need to be implemented
    Task<T?> GetById(Guid id);
    Task<IEnumerable<T>> GetAll();
    Task<T> Add(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}