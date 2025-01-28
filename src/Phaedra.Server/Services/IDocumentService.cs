using Microsoft.AspNetCore.JsonPatch;
using Phaedra.Server.Models.Entities.Documents;
using System.Linq.Expressions;
namespace Phaedra.Server.Services;
public interface IDocumentService<T> where T : class, IDocumentComponent
{
    Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<Document>>? orderBy = null,
        int? page = null,
        int? pageSize = null);

    Task<T> UpdateAsync(int id, JsonPatchDocument<T> patch);
    Task DeleteAsync(int id);
    Task<T> AddAsync(Document document);
}
