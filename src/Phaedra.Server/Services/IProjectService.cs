using Microsoft.AspNetCore.JsonPatch;
using Phaedra.Server.DTO.Project;
using Phaedra.Server.Models.Entities.ProjectEntities;
using System.Linq.Expressions;

namespace Phaedra.Server.Services
{
    public interface IProjectService
    {
        IQueryable<ProjectDto> Get(Expression<Func<Project, bool>>? filter = null,
            Func<IQueryable<Project>, IOrderedQueryable<Project>>? orderBy = null,
            int? page = null,
            int? pageSize = null);
        Task<ProjectDto> AddAsync(CreateProjectDto dto);
        Task<ProjectDto> UpdateAsync(int id, JsonPatchDocument<Project> patch);
        Task<ProjectDto> DeleteAsync(int id);
        IQueryable<ProjectTask> GetTasks(int projectId, 
            Func<Expression<Func<ProjectTask, bool>>>? func = null);
        Task DeleteTaskAsync(int id);
        Task<ProjectTask> UpdateTaskAsync(int id, JsonPatchDocument<ProjectTask> patch);
        
    }
}
