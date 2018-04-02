using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Data;
using Microsoft.EntityFrameworkCore;
using Slickflow.Engine.Business.Entity;
using Slickflow.Scheduler.Entity;

namespace Slickflow.Scheduler.Data
{
    /// <summary>
    /// 任务调度数据上下文
    /// </summary>
    public class JobDbContext : DbContext
    {
        //workflow engine part
        public DbSet<ProcessEntity> Processes { get; set; }
        public DbSet<ProcessInstanceEntity> ProcessInstances { get; set; }
        public DbSet<ActivityInstanceEntity> ActivityInstances { get; set; }
        public DbSet<TransitionInstanceEntity> TransitionInstances { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<LogEntity> Logs { get; set; }

        //business part
        public DbSet<JobEntity> AppFlows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (ConnectionString.DbType == DatabaseTypeEnum.SQLSERVER.ToString())
                optionsBuilder.UseSqlServer(ConnectionString.Value, b => b.UseRowNumberForPaging());
            else if (ConnectionString.DbType == DatabaseTypeEnum.MYSQL.ToString())
                optionsBuilder.UseMySql(ConnectionString.Value);
            else if (ConnectionString.DbType == DatabaseTypeEnum.ORACLE.ToString())
                optionsBuilder.UseOracle(ConnectionString.Value);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
