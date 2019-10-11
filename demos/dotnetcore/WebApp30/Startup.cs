using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebApp30.Accounts;
using WebApp30.Accounts.AppRoles;
using WebApp30.Accounts.AppUsers;

namespace WebApp30
{
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
            services.AddControllers(opt=>opt.Filters.Add(HostAuthorizeFilter.CreateInstance()));

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.ApplicationServices.GetService<MainDbContext>().Initialize();
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
