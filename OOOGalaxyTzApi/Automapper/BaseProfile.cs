using AutoMapper;
using OOOGalaxyTzApi.DTO;
using OOOGalaxyTzApi.Models;

namespace OOOGalaxyTzApi.Automapper
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<BaseModel, BaseDto>().ReverseMap();
        }
    }
}