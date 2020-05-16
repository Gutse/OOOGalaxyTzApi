using OOOGalaxyTzApi.DTO;
using OOOGalaxyTzApi.Models;

namespace OOOGalaxyTzApi.Extensions
{
    public static class DtoExtensions
    {
        public static T? ToModel<T>(this BaseDto? dto) where T : BaseModel, new()
        {
            if (dto is null || dto.Id == 0)
                return null;

            return new T
            {
                Id = dto.Id
            };
        }
    }
}