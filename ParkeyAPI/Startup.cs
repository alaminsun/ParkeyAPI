using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ParkeyAPI.Data;
using ParkeyAPI.Repository.IRepository;
using ParkeyAPI.ParkeyMapper;
using System.Reflection;
using System.IO;
using System;

namespace ParkeyAPI
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();
            services.AddAutoMapper(typeof(ParkeyMappings));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("ParkeyOpenAPISpecNP", new OpenApiInfo 
                { 
                    Title = "Parkey API (National Park)", 
                    Version = "v1",
                    Description = "Udemy Parkey API NP",
                    Contact = new OpenApiContact()
                    {
                        Email = "alaminsun@yahoo.com",
                        Name = "Md. Al-Amin",
                        Url = new Uri("https://www.google.com")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.google.com")
                    }
                });
                c.SwaggerDoc("ParkeyOpenAPISpecTrails", new OpenApiInfo
                {
                    Title = "Parkey API Trails",
                    Version = "v1",
                    Description = "Udemy Parkey API Trails",
                    Contact = new OpenApiContact()
                    {
                        Email = "alaminsun@yahoo.com",
                        Name = "Md. Al-Amin",
                        Url = new Uri("https://www.google.com")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.google.com")
                    }
                });
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                c.IncludeXmlComments(cmlCommentsFullPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/swagger/ParkeyOpenAPISpecNP/swagger.json", "Parkey API NP");
                    option.SwaggerEndpoint("/swagger/ParkeyOpenAPISpecTrails/swagger.json", "Parkey API Trails");

                });
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/ParkeyOpenAPISpecNP/swagger.json", "Parkey API NP"));
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/ParkeyOpenAPISpecTrails/swagger.json", "Parkey API Trails"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
