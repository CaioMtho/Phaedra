using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Data;
using Phaedra.Server.Models.Utilities;

namespace Phaedra.Server.Services;

public class DataService<T, TDto, TCreateDto, TUpdateDto> : IDataService<T, TDto, TCreateDto, TUpdateDto>
    where T : class
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
{
    private readonly DefaultDbContext _dbContext;
    private readonly DbSet<T> _dbSet;
    private readonly IMapper _mapper;
    public DataService(DefaultDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
        _mapper = mapper;
    }

    public IQueryable<TDto> Get(
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

    public async Task<TDto> AddAsync(TCreateDto dto)
    {
        var entity = _mapper.Map<T>(dto);
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<TDto>(entity);
    }

    public async Task<TDto> UpdateAsync(int id, TUpdateDto dto)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("Entity not found.");

        foreach (var property in typeof(TUpdateDto).GetProperties())
        {
            var value = property.GetValue(dto);
            if (value == null) continue;
            var entityProperty = typeof(T).GetProperty(property.Name);
            if (entityProperty != null && entityProperty.CanWrite)
            {
                entityProperty.SetValue(entity, value);
            }
        }

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<TDto>(entity);
    }


    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) throw new KeyNotFoundException("Entity not found.");

        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

}