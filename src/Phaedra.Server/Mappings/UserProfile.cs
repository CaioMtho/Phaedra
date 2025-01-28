using AutoMapper;
using Phaedra.Server.DTO.User;
using Phaedra.Server.Models.Entities.UserEntities;

namespace Phaedra.Server.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
