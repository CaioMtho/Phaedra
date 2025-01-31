using Microsoft.AspNetCore.JsonPatch;
using Phaedra.Server.DTO.User;
using Phaedra.Server.Models.Entities.UserEntities;
using System.Linq.Expressions;

namespace Phaedra.Server.Services
{
    public interface IUserService
    {
        IQueryable<UserDto> Get(Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            int? page = null,
            int? pageSize = null);
        Task<UserDto> UpdateAsync(int id, JsonPatchDocument<User> patch);
        Task<UserDto> AddAsync<CreateUserDto>(CreateUserDto dto);
        Task DeleteAsync(int id);
    }
}