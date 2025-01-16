
namespace Phaedra.Server.Services
{
    public interface IDataService<T> where T : class
    {
        Task<T> FindByIdAsync(int id);
        Task<IEnumerable<T>> FindAllAsync();
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
