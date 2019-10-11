namespace WebApp30.Accounts.AppRoles
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Identity;

    public class AppRole : IdentityRole
    {
        public const string IdEnd = "-0000-0000-0000-00000000000A";

        public AppRole()
        {
        }

        public AppRole(Guid id, Guid concurrencyStamp, string name)
        {
            Id = id.ToString();
            ConcurrencyStamp = concurrencyStamp.ToString();
            Name = name;
            NormalizedName = name;
        }

        public enum NameEnum
        {
            系统管理员,
            管理员,
        }

        public NameEnum? NameToEnum()
        {
            var enumValue = default(NameEnum?);

            if (string.IsNullOrEmpty(Name) || Enum.GetNames(typeof(NameEnum)).Contains(Name) == false)
            {
                return enumValue;
            }

            enumValue = (NameEnum)Enum.Parse(typeof(NameEnum), Name);

            return enumValue;
        }
    }
}
