using System.IO;
using System.Text;
using AutoMapper;
using FutsalSystem.Repository;
using FutsalSystem.Repository.Interface;
using FutsalSystem.Services;
using FutsalSystem.Services.Interfaces;
using FutsalSystem.SharedConfigurations.Hubs;
using FutsalSystem.SharedConfigurations.MappingProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

namespace FutsalSystem
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "mysite.com",
                    ValidAudience = "mysite.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsSecurityKey954785!@!!!"))
                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
                });
            });


            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();

            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddHttpContextAccessor();

            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IMatchEventService, MatchEventService>();
            services.AddScoped<IAnnouncementService, AnnouncementService>();
            services.AddScoped<IRepository, Repository.Repository>();
            services.AddSignalR();


            services.AddDbContext<Context>(options => options.UseSqlServer(Configuration["ConnectionStrings:FutsalSystemConnectionString"]));
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

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Shared/Files/Images")),
                RequestPath = new PathString("/" + Configuration.GetValue<string>("SharedImagesPath"))
            });

            app.UseCors();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseSignalR(routes => { routes.MapHub<ChatHub>("/chatHub"); });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id}");
            });
        }
    }
}
