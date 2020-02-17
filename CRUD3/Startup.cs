using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Account;
using Database;
using Entities;
using IBusiness.Account;
using IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Repositories;

namespace CRUD3
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
            services.AddDbContext<BookStoreContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DevConnection"),
               b => b.MigrationsAssembly("CRUD3")));

            //Add EntityFramework Authentication
            services.AddIdentity<ApplicationUser, IdentityRole>(options => ConfigurePasswordSettings(options))
              .AddEntityFrameworkStores<BookStoreContext>()
              .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(Startup));
            
            services.AddControllersWithViews();

            //services.AddControllersWithViews(config =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //                     .RequireAuthenticatedUser()
            //                     .Build();

            //    config.Filters.Add(new AuthorizeFilter(policy));
            //});

            //Acá se agregan las lineas para la inyección de dependencias (3 formas de agregar dependencias)
            ResolveDependencies(services);

        }

        private static void ResolveDependencies(IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();

            services.AddScoped<IAccountManager, AccountManager>();
        }

        //Configure password validation to create user
        private void ConfigurePasswordSettings(IdentityOptions options)
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 1;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
