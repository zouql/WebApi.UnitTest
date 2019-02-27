namespace WebApp
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using WebApp.Accounts.AppRoles;
    using WebApp.Accounts.AppUsers;

    public class MainDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        internal static void Initialize(MainDbContext context)
        {
            // auto migrate
            context?.Database?.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
