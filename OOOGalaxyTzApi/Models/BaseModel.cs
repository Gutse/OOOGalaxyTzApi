using System;

namespace OOOGalaxyTzApi.Models
{
    public class BaseModel : IComparable
    {
        public virtual int Id { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == this)
                return true;

            return obj is BaseModel model && Id.Equals(model.Id);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public virtual int CompareTo(object? obj)
        {
            if (obj is BaseModel otherModel)
                return string.Compare(Id.ToString(), otherModel.Id.ToString(), StringComparison.CurrentCulture);

            throw new ArgumentException("Object is not a BaseModel");
        }

        public override string ToString() => $"{Id} - {GetType().FullName}";
    }
}