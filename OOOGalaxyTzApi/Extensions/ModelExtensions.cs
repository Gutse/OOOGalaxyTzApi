using OOOGalaxyTzApi.Models;

namespace OOOGalaxyTzApi.Extensions
{
    public static class ModelExtensions
    {
        public static bool IsNullOrEmpty(this BaseModel? model) => model is null || model.Id == 0;
    }
}