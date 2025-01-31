using System.ComponentModel.DataAnnotations;
using Phaedra.Server.Models.Entities.DocumentEntities;
using Phaedra.Server.Models.Entities.UserEntities;
using Phaedra.Server.Models.Shared;

namespace Phaedra.Server.Models.Entities.ProjectEntities;

public class Project : IEntity
{
    [Key]
    public int Id { get; set; }
    [MinLength(3, ErrorMessage = "Project name must be at least 3 characters long.")]
    [MaxLength(255, ErrorMessage = "Project name cannot be longer than 255 characters.")]
    public string Name { get; set; } = "Project";
    [MaxLength(255, ErrorMessage = "Project description cannot be longer than 255 characters.")]
    public string Description { get; set; } = string.Empty;
    public ICollection<ProjectTask> Tasks { get; set; } = [];
    public ICollection<int> DocumentIds { get; set; } = [];
    [Required] 
    public ICollection<int> AuthorIds { get; set; } = [];
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.Working;
}