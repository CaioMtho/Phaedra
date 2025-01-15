using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Data;

namespace Phaedra.Server.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DefaultDbContext _context;
    private readonly DbSet<T> _dbSet;
    public Repository(DefaultDbContext dbContext)
    {
        _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _context.Set<T>() ?? throw new NullReferenceException(nameof(_dbSet));
    }
    
    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id) ?? throw new NullReferenceException(nameof(_dbSet));
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException("Entity not found");
        }
    }
}