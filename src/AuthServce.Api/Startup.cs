using System;
using System.Text;
using AuthService.Domain.Entity;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Configuration;
using AuthService.Infrastructure.Contract;
using AuthService.Infrastructure.Contract.Repository;
using AuthService.Infrastructure.Repository;
using AuthService.Infrastructure.Validator;
using AuthService.Service.Implementation;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace AuthService.Api
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

            //services.AddControllers();
            services.AddHttpClient();
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddLogging(logging =>
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("logs/authservice.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

                logging.AddSerilog();
            });

            services.AddValidatorsFromAssemblyContaining<UserValidation>();
            services.AddTransient<ILocalGovernmentAreaRepository, LocalGovtAreaRepository>();
            services.AddTransient<IStatesRepository, StatesRepository>();
            services.AddTransient<IAuthRegistrationService, AuthRegistrationService>();
            services.AddTransient<IStateService, StatesService>();
            services.AddTransient<ILocalGovtService, LocalGovtService>();
            services.AddTransient<IAuthLoginService, AuthLoginService>();
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<ILoginRepository , LoginRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService.Api", Version = "v1" });
            });
            var conn = Configuration.GetConnectionString("Conn");
            Console.WriteLine($"conn string == {conn}");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    conn, 
                    sqlOptions => sqlOptions.MigrationsAssembly("AuthService.Api")
                );
            });

            services.Configure<JwtConfig>(Configuration.GetSection(key: "JwtConfig"));
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    //ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,  
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });



        }
         
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "UAT" || env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService.Api v1"));
            }

           
            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
