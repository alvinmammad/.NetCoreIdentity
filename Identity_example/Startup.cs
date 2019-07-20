using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Identity_example.Data;
using Microsoft.EntityFrameworkCore;
using Identity_example.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity_example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddIdentity<User, IdentityRole>()
           .AddEntityFrameworkStores<UserIdentityDbContext>()
           .AddDefaultUI()
           .AddDefaultTokenProviders();
            #region Identity Options
            services.Configure<IdentityOptions>(identityOptions =>
            {
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequireLowercase = true;
                identityOptions.Password.RequireUppercase = true;
                identityOptions.Password.RequiredLength = 8;
                identityOptions.Password.RequireNonAlphanumeric = true;
                identityOptions.User.RequireUniqueEmail = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 3;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                identityOptions.Lockout.AllowedForNewUsers = false;
            });
            #endregion

            services.AddDbContext<UserIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionsStrings:Default"]);
            });


            services.AddAuthentication()
               .AddFacebook(facebook =>
               {
                   facebook.AppId = Configuration["OAuth:Facebook:AppId"];
                   facebook.AppSecret = Configuration["OAuth:Facebook:AppSecret"];
               })
               .AddGoogle(google=>
               {
                   google.ClientId = Configuration["OAuth:Google:ClientId"];
                   google.ClientSecret = Configuration["OAuth:Google:ClientSecret"];
               });


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Login";
            });
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
