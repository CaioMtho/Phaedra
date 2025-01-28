using Phaedra.Server.Models.DTO.Users.User;
using Phaedra.Server.Models.Shared;
using Phaedra.Server.Models.DTO.Projects.ProjectTask;

namespace Phaedra.Server.Models.DTO.Projects.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<ProjectTaskDto> Tasks { get; set; } = null!;
        public List<DocumentDto> Documents { get; set; } = null!;
        public List<UserDto> Authors { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status Status { get; set; }
    }
}
