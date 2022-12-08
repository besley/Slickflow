using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Slickflow.Designer
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

            services.AddControllers();
            services.AddMvc(o => o.EnableEndpointRouting = false)
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = null;
                    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
                    o.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    o.JsonSerializerOptions.MaxDepth = 1000;
                });

            services.AddCors(o => o.AddPolicy("MyCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:5000")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


            var dbType = ConfigurationExtensions.GetConnectionString(Configuration, "WfDBConnectionType");
            var sqlConnectionString = ConfigurationExtensions.GetConnectionString(Configuration, "WfDBConnectionString");
            Slickflow.Data.DBTypeExtenstions.InitConnectionString(dbType, sqlConnectionString);
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

            app.UseStaticFiles();

            app.UseCors("MyCorsPolicy");

            app.UseMvc(route =>
            {
                route.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                route.MapRoute(
                    name: "defaultApi",
                    template: "api/{controller}/{action}/{id?}");
            });
        }
    }
}
