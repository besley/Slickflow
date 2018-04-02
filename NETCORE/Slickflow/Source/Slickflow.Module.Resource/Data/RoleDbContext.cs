using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Slickflow.Data;
using Slickflow.Module.Resource.Entity;

namespace Slickflow.Module.Resource.Data
{
    /// <summary>
    /// 角色数据上下文
    /// </summary>
    public class RoleDbContext : DbContext
    {
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleUserEntity> RoleUsers { get; set; }
        public DbSet<DeptEntity> Depts { get; set; }
        public DbSet<EmpEntity> Emps { get; set; }
        public DbSet<EmpMgrEntity> EmpMgrs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (ConnectionString.DbType == DatabaseTypeEnum.SQLSERVER.ToString())
                optionsBuilder.UseSqlServer(ConnectionString.Value, b=>b.UseRowNumberForPaging());
            else if (ConnectionString.DbType == DatabaseTypeEnum.MYSQL.ToString())
                optionsBuilder.UseMySql(ConnectionString.Value);
            else if (ConnectionString.DbType == DatabaseTypeEnum.ORACLE.ToString())
                optionsBuilder.UseOracle(ConnectionString.Value);
        }
    }
}
