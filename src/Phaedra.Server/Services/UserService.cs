using AutoMapper;
using BCrypt;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Phaedra.Server.Data;
using Phaedra.Server.DTO.User;
using Phaedra.Server.Models.Entities.UserEntities;

namespace Phaedra.Server.Services
{
    public class UserService : BaseDataService<User, UserDto>
    {
        
        public UserService(DefaultDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
        public override async Task<UserDto> UpdateAsync(int id, JsonPatchDocument<User> patch)
        {
            if (patch == null)
            {
                throw new ArgumentNullException(nameof(patch), "Patch document cannot be null.");
            }

            if (patch.Operations.Any(op => op.path.Equals("/Id", StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Modifying the 'Id' property is not allowed.");
            }

            var user = await _dbSet.FindAsync(id) ??
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

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }

            return _mapper.Map<UserDto>(user);
        }


        public override async Task<UserDto> AddAsync<CreateUserDto>(CreateUserDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            var user = _mapper.Map<User>(dto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _dbSet.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public override async Task DeleteAsync(int id)
        {
            var user = _dbSet.FirstOrDefault(x => x.Id == id) ?? throw new KeyNotFoundException("User not found");
            user.IsActive = false;
            await _dbContext.SaveChangesAsync();
        }
    }
}
