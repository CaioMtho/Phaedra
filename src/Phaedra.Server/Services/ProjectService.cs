using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Data;
using System.Linq.Expressions;
using Phaedra.Server.DTO.Project;
using Phaedra.Server.Models.Entities.ProjectEntities;

namespace Phaedra.Server.Services
{
    public class ProjectService : BaseDataService<Project, ProjectDto>
    {
        private readonly DbSet<ProjectTask> _tasks;
        public ProjectService(DefaultDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _tasks = dbContext.Set<ProjectTask>();
        }

        // Ao solicitar a exclusão de um Project o que acontece é a mudança de status dele e dos objetos relacionados
        public override async Task<ProjectDto> DeleteAsync(int id)
        {
            var project = await _dbSet.FirstOrDefaultAsync(p => p.Id == id) ?? throw new KeyNotFoundException("Project not found");
            project.Tasks.ForEach(t => t.Status = Models.Shared.Status.Archived);
            project.Status = Models.Shared.Status.Archived;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProjectDto>(project);
        }

        public IQueryable<ProjectTask> GetTasks(int projectId, Func<Expression<Func<ProjectTask, bool>>>? func = null)
        {
            if (func == null)
            {
                return _tasks.AsNoTracking().Where(x => x.ProjectId == projectId)
                    .AsQueryable() ?? throw new KeyNotFoundException("No tasks were found");
            }

            var compiledExpression = func().Compile();
            return _tasks.AsNoTracking().Where(x => compiledExpression(x) && x.ProjectId == projectId)
                .AsQueryable() ?? throw new KeyNotFoundException("Task not found");
        }

        public async Task DeleteTaskAsync(int id) 
        {
            var task = await _tasks
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new KeyNotFoundException("Task not found");

            _tasks.Remove(task);

            await _dbContext.SaveChangesAsync();

        }

        public async Task<ProjectTask> UpdateTaskAsync(int id, JsonPatchDocument<ProjectTask> patch)
        {
            if (patch.Operations.Any(op => op.path.Equals("/Id", StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Modifying the 'Id' property is not allowed.");
            }
            var task = await _tasks
                 .FirstOrDefaultAsync(x => x.Id == id)
                 ?? throw new KeyNotFoundException("Task not found.");
            patch.ApplyTo(task);
            await _dbContext.SaveChangesAsync();
            return task;
        }
    }
}
