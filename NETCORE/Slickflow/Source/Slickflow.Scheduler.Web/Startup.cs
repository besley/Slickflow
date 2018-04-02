using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Slickflow.Data;
using Slickflow.Scheduler.Web.Models;

namespace Slickflow.Scheduler.Web
{
    /// <summary>
    /// 启动类
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 启动方法
        /// </summary>
        /// <param name="env"></param>
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
            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddSingleton<IConfigurationRoot>(Configuration);

            // Get database setting
            var dbType = ConfigurationExtensions.GetConnectionString(Configuration, "WfDBConnectionType");
            var sqlConnectionString = ConfigurationExtensions.GetConnectionString(Configuration, "WfDBConnectionString");

            Slickflow.Data.ConnectionString.DbType = dbType;
            Slickflow.Data.ConnectionString.Value = sqlConnectionString;

            // Add Hangfire service
            services.AddHangfire(config => config.UseSqlServerStorage(Configuration.GetConnectionString("HangFireDBConnectionString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc(route => {
                route.MapRoute(
                    name: "defaultApi",
                    template: "api/{controller}/{action}/{id?}");
            });

            //hangfire dashboard
            JobStorage.Current = new Hangfire.SqlServer.SqlServerStorage(Configuration.GetConnectionString("HangFireDBConnectionString"));
            var schedulerModel = new SchedulerModel();
            schedulerModel.AddSchedulerJob();

            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                AppPath = ""
            });
            app.UseHangfireServer();
        }
    }
}
