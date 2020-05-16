using OOOGalaxyTzApi.Models;
using FluentNHibernate.Mapping;

namespace OOOGalaxyTzApi.Mappings
{
    public class UserMap : ClassMap<UserModel>
    {
        public UserMap()
        {
            Table("users");
            Id(i => i.Id)
                .Column("id")
                .GeneratedBy.Native();
            Map(i => i.Login)
                .Column("login");
            Map(i => i.Password)
                .Column("password");
            Map(i => i.BirthDate)
                .Column("birthdate");
            Map(i => i.Amount)
                .Column("amount");
            Map(i => i.Salt)
                .Column("salt");
        }
    }
}