using System;

namespace OOOGalaxyTzApi.Models
{
    public class UserModel: BaseModel
    {
        /// <summary>
        /// Логин
        /// </summary>
        public virtual string? Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public virtual string? Password { get; set; }

        /// <summary>
        /// Соль
        /// </summary>
        public virtual string? Salt { get; set; }

        /// <summary>
        /// Почта
        /// </summary>
        public virtual string? Email { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime? BirthDate { get; set; }
        
        /// <summary>
        /// Какое то количество
        /// </summary>
        public virtual int Amount { get; set; }
    }
}
