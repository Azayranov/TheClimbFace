using TheClimbFace.Data;
using TheClimbFace.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext context;
    private readonly DbSet<T> dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<T>();
    }

    // public async Task AddAsync(T entity)
    // {
    //     await this.dbSet.AddAsync(entity);
    //     await this.context.SaveChangesAsync();
    // }

    // public async Task<bool> AddRangeAsync(IEnumerable<T> entities)
    // {
    //     if (entities == null)
    //         return false;

    //     dbSet.AddRange(entities);
    //     await context.SaveChangesAsync();
    //     return true;
    // }

    // public async Task<bool> DeleteAsync(Guid id)
    // {
    //     T entity = await GetByIdAsync(id);

    //     if (entity == null)
    //         return false;

    //     dbSet.Remove(entity);
    //     await context.SaveChangesAsync();
    //     return true;
    // }

    // public async Task<bool> DeleteAsync(T entity)
    // {
    //     if (entity == null)
    //         return false;

    //     dbSet.Remove(entity);
    //     await context.SaveChangesAsync();
    //     return true;
    // }

    // public async Task<bool> DeleteRangeAsync(IEnumerable<T> entities)
    // {
    //     if (entities == null)
    //         return false;

    //     dbSet.RemoveRange(entities);
    //     await context.SaveChangesAsync();
    //     return true;
    // }

    // public async Task<IEnumerable<T>> GetAllAsync()
    // {
    //     return await dbSet.ToListAsync();
    // }

    // public IQueryable<T> GetAllAttached()
    // {
    //     return dbSet.AsQueryable();
    // }

    // public async Task<T> GetByIdAsync(Guid id)
    // {
    //     return await dbSet.FindAsync(id);
    // }

    // public async Task UpdateAsync(T entity)
    // {
    //     dbSet.Attach(entity);
    //     context.Entry(entity).State = EntityState.Modified;
    //     await context.SaveChangesAsync();
    // }
}