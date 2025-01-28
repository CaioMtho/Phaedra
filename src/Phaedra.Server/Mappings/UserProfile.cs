using AutoMapper;
using Phaedra.Server.Models.DTO.Users.User;
using Phaedra.Server.Models.Entities.Users;

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
