using AutoMapper;
using Phaedra.Server.Models.DTO.Projects.Project;
using Phaedra.Server.Models.Entities.Projects;

namespace Phaedra.Server.Mappings
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile() 
        {
            CreateMap<Project, ProjectDto>();
        }
    }

}
