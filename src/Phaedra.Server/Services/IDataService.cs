using System.Linq.Expressions;
using Phaedra.Server.Models.Utilities;

namespace Phaedra.Server.Services;

public interface IDataService<T, TDto, in TCreateDto, in TUpdateDto> 
    where T : class 
    where TDto : class 
    where TCreateDto : class 
    where TUpdateDto : class
{
    IQueryable<TDto> Get(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? page = null,
        int? pageSize = null);
    Task DeleteAsync(int id);
    Task<TDto> AddAsync(TCreateDto dto);
    Task<TDto> UpdateAsync(int id,TUpdateDto dto);
    
}