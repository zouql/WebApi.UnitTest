namespace WebApp30.Accounts.AppUsers
{
    using Microsoft.AspNetCore.Identity;

    public class AppUser : IdentityUser
    {
        public const string IdEnd = "-0000-0000-0000-00000000000B";

        public string Name { get; set; }
    }
}
