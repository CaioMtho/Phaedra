using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;

namespace Phaedra.Server.Services;

public interface IDataService<T, TDto> 
    where T : class 
    where TDto : class 
{
    IQueryable<TDto> Get(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? page = null,
        int? pageSize = null);
    Task DeleteAsync(int id);
    Task<TDto> AddAsync<TCreateDto>(TCreateDto dto);
    Task<TDto> UpdateAsync(int id, JsonPatchDocument<T> patch);


}