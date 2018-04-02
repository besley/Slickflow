using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Slickflow.Engine.Business.Entity;
using Slickflow.Module.Resource.Entity;

namespace Slickflow.EFCoreMgr.Data
{
    /// <summary>
    /// 流程数据上下文
    /// </summary>
    public class WfDbContextBeta : DbContext
    {
        //workflow service data
        public DbSet<ProcessEntity> Processes { get; set; }
        public DbSet<ProcessInstanceEntity> ProcessInstances { get; set; }
        public DbSet<ActivityInstanceEntity> ActivityInstances { get; set; }
        public DbSet<TransitionInstanceEntity> TransitionInstances { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<LogEntity> Logs { get; set; }

        //role service data
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<RoleUserEntity> RoleUsers { get; set; }
        public DbSet<DeptEntity> Depts { get; set; }
        public DbSet<EmpEntity> Emps { get; set; }
        public DbSet<EmpMgrEntity> EmpMgrs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionStringBeta.Value);
        }

        /// <summary>
        /// 数据模型创建过程
        /// </summary>
        /// <param name="modelBuilder">模型构建</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //忽略任务视图
            modelBuilder.Ignore<TaskViewEntity>();
            modelBuilder.Ignore<RoleUserView>();

            //默认值赋值
            SetDefaultValueforDatabase(modelBuilder, ConnectionStringBeta.DbType);
        }

        private void SetDefaultValueforDatabase(ModelBuilder modelBuilder, string dbType)
        {
            //流程创建
            modelBuilder.Entity<ProcessEntity>(entity =>
            {
                entity.Property(e => e.Version).HasDefaultValue("1");
                entity.Property(e => e.IsUsing).HasDefaultValue(0);
                entity.Property(e => e.StartType).HasDefaultValue(0);
                entity.Property(e => e.EndType).HasDefaultValue(0);
                entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("getdate()");
            });

            //流程实例创建
            modelBuilder.Entity<ProcessInstanceEntity>(entity =>
            {
                entity.Property(e => e.Version).HasDefaultValue("1");
                entity.Property(e => e.ProcessState).HasDefaultValue(0);
                entity.Property(e => e.ParentProcessInstanceID).HasDefaultValue(0);
                entity.Property(e => e.InvokedActivityInstanceID).HasDefaultValue(0);
                entity.Property(e => e.RecordStatusInvalid).HasDefaultValue(0);
                entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("getdate()");
            });

            //活动实例
            modelBuilder.Entity<ActivityInstanceEntity>(entity =>
            {
                entity.Property(e => e.ActivityState).HasDefaultValue(0);
                entity.Property(e => e.WorkItemType).HasDefaultValue(0);
                entity.Property(e => e.CanRenewInstance).HasDefaultValue(0);
                entity.Property(e => e.TokensRequired).HasDefaultValue(1);
                entity.Property(e => e.RecordStatusInvalid).HasDefaultValue(0);
                entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("getdate()");
            });

            //转移实例
            modelBuilder.Entity<TransitionInstanceEntity>(entity => {
                entity.Property(e => e.FlyingType).HasDefaultValue(0);
                entity.Property(e => e.ConditionParseResult).HasDefaultValue(0);
                entity.Property(e => e.RecordStatusInvalid).HasDefaultValue(0);
                entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("getdate()");
            });

            //任务实例
            modelBuilder.Entity<TaskEntity>(entity => {
                entity.Property(e => e.TaskState).HasDefaultValue(0);
                entity.Property(e => e.RecordStatusInvalid).HasDefaultValue(0);
                entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("getdate()");
            });
            
            //else if(dbType == DatabaseTypeEnum.ORACLE.ToString())
            //{
            //    //流程创建
            //    modelBuilder.Entity<ProcessEntity>(entity =>
            //    {
            //        entity.Property(e => e.Version).HasDefaultValue("1");
            //        entity.Property(e => e.IsUsing).HasDefaultValue(0);
            //        entity.Property(e => e.StartType).HasDefaultValue(0);
            //        entity.Property(e => e.EndType).HasDefaultValue(0);
            //        entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            //    });

            //    //流程实例创建
            //    modelBuilder.Entity<ProcessInstanceEntity>(entity =>
            //    {
            //        entity.Property(e => e.Version).HasDefaultValue("1");
            //        entity.Property(e => e.ProcessState).HasDefaultValue(0);
            //        entity.Property(e => e.ParentProcessInstanceID).HasDefaultValue(0);
            //        entity.Property(e => e.InvokedActivityInstanceID).HasDefaultValue(0);
            //        entity.Property(e => e.RecordStatusInvalid).HasDefaultValue(0);
            //        entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            //    });

            //    //活动实例
            //    modelBuilder.Entity<ActivityInstanceEntity>(entity =>
            //    {
            //        entity.Property(e => e.ActivityState).HasDefaultValue(0);
            //        entity.Property(e => e.WorkItemType).HasDefaultValue(0);
            //        entity.Property(e => e.CanRenewInstance).HasDefaultValue(0);
            //        entity.Property(e => e.TokensRequired).HasDefaultValue(1);
            //        entity.Property(e => e.RecordStatusInvalid).HasDefaultValue(0);
            //        entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            //    });

            //    //转移实例
            //    modelBuilder.Entity<TransitionInstanceEntity>(entity => {
            //        entity.Property(e => e.FlyingType).HasDefaultValue(0);
            //        entity.Property(e => e.ConditionParseResult).HasDefaultValue(0);
            //        entity.Property(e => e.RecordStatusInvalid).HasDefaultValue(0);
            //        entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            //    });

            //    //任务实例
            //    modelBuilder.Entity<TaskEntity>(entity => {
            //        entity.Property(e => e.TaskState).HasDefaultValue(0);
            //        entity.Property(e => e.RecordStatusInvalid).HasDefaultValue(0);
            //        entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            //    });
            //}
        }
    }
}