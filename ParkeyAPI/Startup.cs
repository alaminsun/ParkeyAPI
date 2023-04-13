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
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

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
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("ParkeyOpenAPISpec", new OpenApiInfo 
            //    { 
            //        Title = "Parkey API", 
            //        Version = "v1",
            //        Description = "Udemy Parkey API",
            //        Contact = new OpenApiContact()
            //        {
            //            Email = "alaminsun@yahoo.com",
            //            Name = "Md. Al-Amin",
            //            Url = new Uri("https://www.google.com")
            //        },
            //        License = new OpenApiLicense()
            //        {
            //            Name = "MIT License",
            //            Url = new Uri("https://www.google.com")
            //        }
            //    });
            //    //c.SwaggerDoc("ParkeyOpenAPISpecTrails", new OpenApiInfo
            //    //{
            //    //    Title = "Parkey API Trails",
            //    //    Version = "v1",
            //    //    Description = "Udemy Parkey API Trails",
            //    //    Contact = new OpenApiContact()
            //    //    {
            //    //        Email = "alaminsun@yahoo.com",
            //    //        Name = "Md. Al-Amin",
            //    //        Url = new Uri("https://www.google.com")
            //    //    },
            //    //    License = new OpenApiLicense()
            //    //    {
            //    //        Name = "MIT License",
            //    //        Url = new Uri("https://www.google.com")
            //    //    }
            //    //});
            //    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //    c.IncludeXmlComments(cmlCommentsFullPath);
            //});

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var desc in provider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", 
                            desc.GroupName.ToUpperInvariant());
                        //options.RoutePrefix = "";
                    
                });
                //app.UseSwaggerUI(options =>
                //{
                //    options.SwaggerEndpoint("/swagger/ParkeyOpenAPISpec/swagger.json", "Parkey API");
                //    //option.SwaggerEndpoint("/swagger/ParkeyOpenAPISpecTrails/swagger.json", "Parkey API Trails");
                //    options.RoutePrefix = "";
                //});
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
