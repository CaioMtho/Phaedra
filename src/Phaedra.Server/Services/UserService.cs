using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Data;
using Phaedra.Server.DTO.User;
using Phaedra.Server.Models.Entities.UserEntities;

namespace Phaedra.Server.Services
{
    public class UserService : IUserService
    {
        private readonly DefaultDbContext _context;
        private readonly IMapper _mapper;
        private readonly DbSet<User> _users;

        public UserService(DefaultDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _users = _context.Users;
            _mapper = mapper;
        }

        public IQueryable<UserDto> Get(
        Expression<Func<User, bool>>? filter = null,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
        int? page = null,
        int? pageSize = null)
        {
            var query = filter != null ? _users.Where(filter) : _users.AsQueryable();

            if (orderBy != null)
                query = orderBy(query);

            if (page.HasValue && pageSize.HasValue)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            return _mapper.ProjectTo<UserDto>(query);
        }

        public async Task<UserDto> UpdateAsync(int id, JsonPatchDocument<User> patch)
        {
            if (patch == null)
            {
                throw new ArgumentNullException(nameof(patch), "Patch document cannot be null.");
            }

            if (patch.Operations.Any(op => op.path.Equals("/Id", StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Modifying the 'Id' property is not allowed.");
            }

            var user = await _users.FindAsync(id) ??
                throw new KeyNotFoundException($"User with ID {id} not found.");

            if (patch.Operations.Any(op => op.path.Equals("/Password", StringComparison.OrdinalIgnoreCase)))
            {
                var passwordOperation = patch.Operations
                     .First(op => op.path.Equals("/Password", StringComparison.OrdinalIgnoreCase));

                if (passwordOperation.value is not string newPassword || string.IsNullOrWhiteSpace(newPassword))
                {
                    throw new InvalidOperationException("Password cannot be null or empty.");
                }

                patch.ApplyTo(user);
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            }
            else
            {
                patch.ApplyTo(user);
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<UserDto>(user);
        }


        public async Task<UserDto> AddAsync<CreateUserDto>(CreateUserDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            var user = _mapper.Map<User>(dto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _users.AddAsync(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id) ?? throw new KeyNotFoundException("User not found");
            user.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
