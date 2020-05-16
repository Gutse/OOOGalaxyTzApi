using AutoMapper;
using OOOGalaxyTzApi.DTO;
using OOOGalaxyTzApi.Models;

namespace OOOGalaxyTzApi.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, UserDto>().ReverseMap();
            CreateMap<UserModel, BaseDto>().ReverseMap();
        }
    }
}