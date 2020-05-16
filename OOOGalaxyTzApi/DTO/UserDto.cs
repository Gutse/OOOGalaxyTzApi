using System;

namespace OOOGalaxyTzApi.DTO
{
    public class UserDto : BaseDto
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string? Login { get; set; }

        /// <summary>
        /// Почта
        /// </summary>
        public string? Email { get; set; }
        
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? BirthDate { get; set; }
        
        /// <summary>
        /// Какое то количество
        /// </summary>
        public int Amount { get; set; }
    }
}