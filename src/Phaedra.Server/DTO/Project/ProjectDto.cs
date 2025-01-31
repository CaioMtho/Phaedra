using Phaedra.Server.DTO.User;
using Phaedra.Server.Models.Entities.DocumentEntities;
using Phaedra.Server.Models.Entities.ProjectEntities;
using Phaedra.Server.Models.Shared;

namespace Phaedra.Server.DTO.Project
{
    public class ProjectDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string Description { get; init; } = null!;
        public ICollection<ProjectTask> Tasks { get; set; } = null!;
        public ICollection<int> DocumentIds { get; set; } = null!;
        public ICollection<int> AuthorIds { get; init; } = null!;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public Status Status { get; init; }
    }
}
