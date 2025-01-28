using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Models.Entities.ProjectEntities;
using Phaedra.Server.Models.Entities.UserEntities;

namespace Phaedra.Server.Data;

public class DefaultDbContext(DbContextOptions<DefaultDbContext> options) 
    : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<User> Users { get; set; }
}