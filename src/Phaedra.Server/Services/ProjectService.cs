using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Data;
using System.Linq.Expressions;
using Phaedra.Server.DTO.Project;
using Phaedra.Server.Models.Entities.ProjectEntities;
using Microsoft.AspNetCore.Mvc;

namespace Phaedra.Server.Services
{
    public class ProjectService : IProjectService
    {
        private readonly DbSet<Project> _projects;
        private readonly IMapper _mapper;
        private readonly DbSet<ProjectTask> _tasks;
        private readonly DefaultDbContext _context;
        public ProjectService(DefaultDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _projects = _context.Projects;
            _tasks = _context.Set<ProjectTask>();
            _mapper = mapper;
        }

        public IQueryable<ProjectDto> Get(
        Expression<Func<Project, bool>>? filter = null,
        Func<IQueryable<Project>, IOrderedQueryable<Project>>? orderBy = null,
        int? page = null,
        int? pageSize = null)
        {
            var query = filter != null ? _projects.Where(filter) : _projects.AsQueryable();

            if (orderBy != null)
                query = orderBy(query);

            if (page.HasValue && pageSize.HasValue)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            return _mapper.ProjectTo<ProjectDto>(query);
        }

        // Ao solicitar a exclusão de um Project o que acontece é a mudança de status dele e dos objetos relacionados
        public async Task<ProjectDto> DeleteAsync(int id)
        {
            var project = await _projects.FirstOrDefaultAsync(p => p.Id == id) ?? 
                throw new KeyNotFoundException("Project not found");
            project.Tasks.ForEach(t => t.Status = Models.Shared.Status.Archived);
            project.Status = Models.Shared.Status.Archived;
            await _context.SaveChangesAsync();
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

            await _context.SaveChangesAsync();

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
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<ProjectDto> AddAsync(CreateProjectDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            var project = _mapper.Map<Project>(dto);
            await _projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProjectDto>(dto);
        }

        public async Task<ProjectDto> UpdateAsync(int id, JsonPatchDocument<Project> patch)
        {
            if (patch == null)
            {
                throw new ArgumentNullException(nameof(patch), "Patch document cannot be null.");
            }

            if (patch.Operations.Any(op => op.path.Equals("/Id", StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Modifying the 'Id' property is not allowed.");
            }

            var project = await _projects.FindAsync(id) ??
                throw new KeyNotFoundException($"User with ID {id} not found.");

            patch.ApplyTo(project);

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<ProjectDto>(project);
        }
    }
}
