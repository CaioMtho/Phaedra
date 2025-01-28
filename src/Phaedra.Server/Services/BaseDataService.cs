using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Data;
using Phaedra.Server.Models.Shared;
using System.Linq.Expressions;

namespace Phaedra.Server.Services;

public abstract class BaseDataService<T, TDto> : IDataService<T, TDto>
    where T : class, IEntity
    where TDto : class
{
    protected readonly DefaultDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;
    protected readonly IMapper _mapper;

    protected BaseDataService(DefaultDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
        _mapper = mapper;
    }

    public virtual IQueryable<TDto> Get(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? page = null,
            int? pageSize = null)
    {
        var query = filter != null ? _dbSet.Where(filter) : _dbSet.AsQueryable();

        if (orderBy != null)
            query = orderBy(query);

        if (page.HasValue && pageSize.HasValue)
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

        return _mapper.ProjectTo<TDto>(query);
    }


    public virtual async Task<TDto> AddAsync<TCreateDto>(TCreateDto dto)
    {
        var entity = _mapper.Map<T>(dto);
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<TDto> UpdateAsync(int id, JsonPatchDocument<T> patch)
    {
        ArgumentNullException.ThrowIfNull(patch, "Patch can't be null");
        if (patch.Operations.Any(op => op.path.Equals("/Id", StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Modifying the 'Id' property is not allowed.");
        }
        var entity = await _dbSet.FindAsync(id) ?? 
            throw new KeyNotFoundException($"{typeof(T)} not found");
        patch.ApplyTo(entity);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<TDto>(entity);
    }


    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id) ?? 
            throw new KeyNotFoundException("Entity not found.");
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

}