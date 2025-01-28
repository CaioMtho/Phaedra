using System.ComponentModel.DataAnnotations;
using Phaedra.Server.Models.Shared;

namespace Phaedra.Server.Models.Entities.ProjectEntities;

public class ProjectTask : IEntity
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

    private Status _status = Status.Working;

    public Status Status
    {
        get => _status;
        set
        {
            if (value == Status.Done && _status != Status.Done)
            {
                EndDate = DateTime.UtcNow;
            }
            _status = value;
        }
    }

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    public DateTime? EndDate { get; set; }
}
