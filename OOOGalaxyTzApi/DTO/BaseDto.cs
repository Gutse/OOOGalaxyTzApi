using System;
using System.ComponentModel;

namespace OOOGalaxyTzApi.DTO
{
    public class BaseDto : IComparable
    {
        public virtual int Id { get; set; }

        public virtual string Text
        {
            get
            {
                var prop = GetType()
                          .GetProperty("Name")
                         ?.GetValue(this, null);
                if (prop == null)
                    return string.Empty;

                return prop.ToString();
            }
        }

        [DisplayName("Id")]
        public virtual string IdToString => Id.ToString();

        public override string ToString() => $"{Id} - {GetType().FullName}";

        public virtual int CompareTo(object obj)
        {
            if (obj is BaseDto otherDto)
                return string.Compare(Id.ToString(), otherDto.Id.ToString(), StringComparison.CurrentCulture);

            throw new ArgumentException("Object is not a BaseDto");
        }

        /// <summary>
        /// Объект имеет Id == 0, значит не сохранен в БД. 
        /// </summary>
        public bool IsNew => Id == 0;
    }
}