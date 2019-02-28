namespace WebApp
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using WebApp.Accounts;
    using WebApp.Accounts.AppRoles;
    using WebApp.Accounts.AppUsers;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt => opt.Filters.Add(HostAuthorizeFilter.CreateInstance())).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContextPool<MainDbContext>(opt => opt.UseSqlite(Configuration.GetConnectionString("ConnectionString")));

            // Add Identity
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<MainDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<AppUserManager>();

            services.AddScoped<AppRoleManager>();

            services.AddScoped<AppSignInManager>();

            services.AddScoped<AccountService>();

            services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt => opt.TokenValidationParameters = new TokenValidationParameters());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private class HostAuthorizeFilter : AuthorizeFilter
        {
            private HostAuthorizeFilter(AuthorizationPolicy policy) : base(policy)
            {
            }

            public static HostAuthorizeFilter CreateInstance()
            {
                var identityApplicationSchemes = "Identity.Application";

                var policy = new AuthorizationPolicyBuilder(new[] { identityApplicationSchemes, JwtBearerDefaults.AuthenticationScheme })
                        .RequireAuthenticatedUser()
                        .Build();

                return new HostAuthorizeFilter(policy);
            }
        }
    }
}
