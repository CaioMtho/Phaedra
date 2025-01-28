using AutoMapper;
using Phaedra.Server.DTO.Project;
using Phaedra.Server.Models.Entities.ProjectEntities;

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
