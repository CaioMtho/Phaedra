using System.ComponentModel.DataAnnotations;
using Phaedra.Server.Models.Shared;

namespace Phaedra.Server.Models.Entities.Projects;

public class ProjectTask
{
    [Key]
    public int Id { get; set; }
    [MinLength(3, ErrorMessage = "Project name must have at least 3 characters")]
    [MaxLength(50, ErrorMessage = "Project name cannot be longer than 50 characters.")]
    public string Name { get; set; } = "Task";
    [MinLength(3, ErrorMessage = "Project description must have at least 3 characters")]
    [MaxLength(255, ErrorMessage = "Project description cannot be longer than 255 characters.")]
    public string Description { get; set; } = string.Empty;
    [Required]
    public int ProjectId { get; set; }
    public Status Status { get; set; } = Status.Working;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
}