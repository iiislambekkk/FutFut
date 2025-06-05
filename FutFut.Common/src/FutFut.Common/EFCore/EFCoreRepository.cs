using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace FutFut.Common.EfCore;

public class EFCoreRepository<TEntity, TDbContext> : IRepository<TEntity> where TEntity : class, IEntity where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public EFCoreRepository(TDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    
    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
    {
        return (await _dbSet.ToListAsync());
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
    {
        return (await _dbSet.Where(filter).ToListAsync());
    }

    public async Task<TEntity?> GetAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet.Where(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        var existing = await _dbSet.FindAsync(entity.Id);
        if (existing == null) throw new Exception("Not found");

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _dbSet.Where(item => item.Id == id).ExecuteDeleteAsync();
    }
}