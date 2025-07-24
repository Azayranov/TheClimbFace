namespace TheClimbFace.Data.Repository.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    IQueryable<T> GetAllAttached();

    Task<T> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task<bool> AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> DeleteAsync(T entity);
    Task<bool> DeleteRangeAsync(IEnumerable<T> entities);

}