namespace WebApp30
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using WebApp30.Accounts.AppRoles;
    using WebApp30.Accounts.AppUsers;

    public class MainDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        internal void Initialize()
        {
            // auto migrate
            Database?.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
