using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Owin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Hangfire;
using Slickflow.Scheduler.Web.Models;

namespace Slickflow.Scheduler.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Hangfire service.
            services.AddHangfire(config =>
                config.UseSqlServerStorage(Configuration.GetConnectionString("HangFireDBConnectionString")));


            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddSingleton<IConfigurationRoot>(Configuration);

            var dbType = ConfigurationExtensions.GetConnectionString(Configuration, "WfDBConnectionType");
            var sqlConnectionString = ConfigurationExtensions.GetConnectionString(Configuration, "WfDBConnectionString");
            Slickflow.Data.DBTypeExtenstions.InitConnectionString(dbType, sqlConnectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //GlobalConfiguration.Configuration.UseSqlServerStorage("HangFireDBConnectionString");
            var options = new DashboardOptions { AppPath = "http://localhost/sfsweb/" };

            //var schedulerModel = new SchedulerModel();
            //schedulerModel.AddSchedulerJob();

            app.UseHangfireDashboard("/jobs", options);
            app.UseHangfireServer();
        }
    }
}



