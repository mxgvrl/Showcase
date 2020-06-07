using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppleShowcase.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;

namespace AppleShowcase {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddTransient<ProductService>();
            services.AddTransient<UserService>();
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Auth/Login");
                });
            services.AddAuthorization(opts => {
                opts.AddPolicy("OnlyForAdmin", policy => {
                    policy.RequireClaim("name", "Admin");
                });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
                app.UseStaticFiles();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization(); 
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "ProductsList",
                    pattern: "{controller=Home}/{action=Home}/{id?}");
            });
        }
    }
}