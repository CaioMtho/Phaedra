using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Models.DTO.Users.User;
using Phaedra.Server.Models.Entities.Users;
using Phaedra.Server.Models.Utilities;
using Phaedra.Server.Services;

namespace Phaedra.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(UserService dataService) : ControllerBase
{
    private readonly UserService _dataService = dataService;

    [HttpGet]
    public async Task<ActionResult> Get(
    [FromQuery] Expression<Func<User, bool>>? filter = null,
    [FromQuery] Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
    [FromQuery] int? page = null,
    [FromQuery] int? pageSize = null)
    {
        pageSize = pageSize > 0 ? Math.Min(pageSize.Value, 100) : 10;
        page ??= 1;

        var query = _dataService.Get(filter, orderBy, page, pageSize);

        var paginatedList = new PaginatedList<UserDto>(query, page.Value, pageSize.Value);
        return Ok(paginatedList);
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        var user = await _dataService.Get(u => u.Id == id).FirstOrDefaultAsync() ?? throw new KeyNotFoundException("User not found");
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Post([FromBody] CreateUserDto userDto)
    {
        var user = await _dataService.AddAsync(userDto);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<UserDto>> Patch(int id, [FromBody] JsonPatchDocument<User> patchDoc)
    {
        ArgumentNullException.ThrowIfNull(patchDoc);
        var user = await _dataService.UpdateAsync(id, patchDoc);
        return Ok(user);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<UserDto>> Delete(int id)
    {
        await _dataService.DeleteAsync(id);
        return NoContent();
    }
}