using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Driver;
using Phaedra.Server.Data;
using Phaedra.Server.Models.Entities.Documents;
using System.Linq.Expressions;

namespace Phaedra.Server.Services
{
    public class DocumentService<T>(DocumentContext context) where T : class, IDocumentComponent
    {
        private readonly IMongoCollection<T> _collection = context.GetCollection<T>();
        public async Task<T> AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _collection.DeleteOneAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? page = null,
        int? pageSize = null)
        {
            var query = _collection.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await Task.FromResult(query.ToList());
        }

        public async Task<T> UpdateAsync(int id, JsonPatchDocument<T> patch)
        {
            var cursor = await _collection.FindAsync(e => e.Id == id);
            var entity = await cursor.FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Entity with id {id} not found");
            patch.ApplyTo(entity);
            await _collection.ReplaceOneAsync(e => e.Id == id, entity);
            return entity;

        }
    }
}
