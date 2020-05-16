using System.Collections.Generic;
using AutoMapper;

namespace OOOGalaxyTzApi.Extensions
{
    /// <summary>
    /// Extensions for Object
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Смаппить в List
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static List<T> MapToListOf<T>(this object data, IMapper mapper)
        {
            return mapper.Map<List<T>>(data);
        }

        /// <summary>
        /// Смаппить в T
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static T MapTo<T>(this object data, IMapper mapper)
        {
            return mapper.Map<T>(data);
        }
    }
}