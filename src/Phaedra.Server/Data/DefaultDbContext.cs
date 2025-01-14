using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Models.Entities.Projects;
using Phaedra.Server.Models.Entities.Users;
using Phaedra.Server.Models.Entities.Documents;

namespace Phaedra.Server.Data;

public class DefaultDbContext(DbContextOptions<DefaultDbContext> options) 
    : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<ContentBlock> ContentBlocks { get; set; }
    public DbSet<TextSegment> TextSegments { get; set; }
    public DbSet<TextStyle> TextStyles { get; set; }
}