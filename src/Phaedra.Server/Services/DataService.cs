using Phaedra.Server.Repositories;

namespace Phaedra.Server.Services
{
    public class DataService<T>(IRepository<T> repository) : IDataService<T> where T : class
    {
        private readonly IRepository<T> _repository = repository;

        public async Task<T> FindByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}