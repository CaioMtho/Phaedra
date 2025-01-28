using System.ComponentModel.DataAnnotations;
using Phaedra.Server.Models.Entities.Documents;
using Phaedra.Server.Models.Entities.Users;
using Phaedra.Server.Models.Shared;

namespace Phaedra.Server.Models.Entities.Projects;

public class Project : IEntity
{
    [Key]
    public int Id { get; set; }
    [MinLength(3, ErrorMessage = "Project name must be at least 3 characters long.")]
    [MaxLength(255, ErrorMessage = "Project name cannot be longer than 255 characters.")]
    public string Name { get; set; } = "Project";
    [MaxLength(255, ErrorMessage = "Project description cannot be longer than 255 characters.")]
    public string Description { get; set; } = string.Empty;
    public List<ProjectTask> Tasks { get; set; } = [];
    public List<Document> Documents { get; set; } = [new Document()];
    [Required] 
    public List<User> Authors { get; set; } = [];
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.Working;
}