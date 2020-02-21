using AutoMapper;
using Business;
using Business.Account;
using Database;
using Entities;
using IBusiness;
using IBusiness.Account;
using IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Repositories;
using System;
using System.Text;

namespace API
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
            //Allow calls from ajax
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            //Adding DatabaseContext. MigratinoAssembly changes the migration startup project (necessary for migrations)
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DevConnection"),
                b => b.MigrationsAssembly("API")));

            //Add EntityFramework Authentication
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => ConfigurePasswordSettings(options))
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            //Add JWT authentication to handle authentication agains db and entityFramework
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "yourdomain.com", //domains that can give authorizations
                        ValidAudience = "yourdomain.com", //domains that can recieve authorizations
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Secret"]))
                        //Above: Using environment variable to produce token. (see API project properties -> debug)
                    });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                //Document swagger version
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookStore", Version = "v1" });

                //Adding JWT authentication with Bearers, this shows an "Authorize" button in Swagger. 
                //We have to logIn, recieve the token and put the token in Authorize button like this: Bearer {token} (do not forget "Bearer" at the begining)
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                //Adding JWT requirement
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                });
            });

            services.AddAutoMapper(typeof(Startup));

            //We add the controllers and also we call Json configuration to handle loops automatically
            services.AddControllers().AddNewtonsoftJson(ConfigureJson);

            //Scoped objects are the same within a request, but different across different requests. (Also can add "AddSingleton" and "AddTransient")
            //AddTransient -> generetares a new instance for every injection. AddSingleton -> Uses always the same instance
            ResolveDependencies(services);
        }

        private static void ResolveDependencies(IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookOperationRepository, BookOperationRepository>();

            services.AddScoped<IBookManager, BookManager>();
            services.AddScoped<IAccountManager, AccountManager>();
        }

        //Configure password validation to create user
        private void ConfigurePasswordSettings(IdentityOptions options)
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 1;

            options.Lockout.MaxFailedAccessAttempts = 10;

            //options.User.RequireUniqueEmail = true;
        }

        //Configuring Json to handle loops automatically
        private void ConfigureJson(MvcNewtonsoftJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore V1");
            });

            //if (!context.Books.Any())
            //{
            //    var rowlingAuthor = new Author() { Name = "J. K. Rowling" };
            //    var tolkienAuthor = new Author() { Name = "J. R. R. Tolkien" };
            //    var marquezAuthor = new Author() { Name = "Garcia Marquez" };
            //    var witcherAuthor = new Author() { Name = "Andrzej Sapkowski" };
            //    var principitoAuthor = new Author() { Name = "Antoine de Saint-Exupery" };

            //    var franClient = new Client() { Name = "Fran" };
            //    var diegoClient = new Client() { Name = "Diego" };

            //    context.Books.AddRange(new List<Book>(){
            //        new Book() { Name = "Harry Potter y la Camara Secreta", Price = 10, Stock = 200, Author = rowlingAuthor },
            //        new Book() { Name = "Harry Potter y la Piedra Filosofal", Price = 20, Stock = 220, Author = rowlingAuthor },
            //        new Book() { Name = "El Señor de los Anillos: La Comunidad del Anillo", Price = 30, Stock = 30, Author = tolkienAuthor },
            //        new Book() { Name = "El Principito", Price = 40, Stock = 150, Author = principitoAuthor },
            //        new Book() { Name = "The Witcher", Price = 50, Stock = 50, Author = witcherAuthor },
            //    });

            //    context.SaveChanges();
            //}
        }
    }
}
