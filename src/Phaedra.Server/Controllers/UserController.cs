using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Models.DTO.User;
using Phaedra.Server.Models.Entities.Users;
using Phaedra.Server.Models.Utilities;
using Phaedra.Server.Services;

namespace Phaedra.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IDataService<User, UserDto, CreateUserDto, UpdateUserDto> dataService) : ControllerBase
{
    private readonly IDataService<User,UserDto,CreateUserDto,UpdateUserDto> _dataService = dataService;

    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] Expression<Func<User, bool>>? filter = null,
        [FromQuery] Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
        [FromQuery] int? page = null,
        [FromQuery] int? pageSize = null)
    {
        pageSize = pageSize > 0 ? Math.Min(pageSize.Value, 100) : 10;

        page = page ?? 1;

        var query = _dataService.Get(filter);
        if (page.HasValue)
        {
            var paginatedList = new PaginatedList<UserDto>(query, page.Value, pageSize.Value);
            return Ok(paginatedList);
        }
        else
        {
            var users = await _dataService.Get(filter, orderBy).ToListAsync();
            return Ok(users);
        }
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        var user = await _dataService.Get(u => u.Id == id).FirstOrDefaultAsync() ?? throw new KeyNotFoundException();
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Post([FromBody] CreateUserDto userDto)
    {
        var user = await _dataService.AddAsync(userDto);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Put(int id, UpdateUserDto userDto)
    {
        var user = await _dataService.UpdateAsync(id, userDto);
        return Ok(user);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<UserDto>> Delete(int id)
    {
        await _dataService.DeleteAsync(id);
        return NoContent();
    }

}