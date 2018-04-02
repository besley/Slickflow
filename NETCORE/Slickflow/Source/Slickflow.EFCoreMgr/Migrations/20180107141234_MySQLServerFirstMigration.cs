using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Slickflow.EFCoreMgr.Migrations
{
    public partial class MySQLServerFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysDepartment",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeptCode = table.Column<string>(nullable: true),
                    DeptName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ParentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDepartment", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysEmployee",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeptID = table.Column<int>(nullable: false),
                    EMail = table.Column<string>(nullable: true),
                    EmpCode = table.Column<string>(nullable: true),
                    EmpName = table.Column<string>(nullable: true),
                    ManagerID = table.Column<int>(nullable: false),
                    Mobile = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysEmployee", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysEmployeeManager",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmpUserID = table.Column<int>(nullable: false),
                    EmployeeID = table.Column<int>(nullable: false),
                    ManagerID = table.Column<int>(nullable: false),
                    MgrUserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysEmployeeManager", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysRole",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleCode = table.Column<string>(nullable: true),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRole", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysRoleUser",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRoleUser", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysUser",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUser", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WfActivityInstance",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActivityGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActivityState = table.Column<short>(nullable: false, defaultValue: (short)0),
                    ActivityType = table.Column<short>(nullable: false),
                    AppInstanceID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssignedToUserIDs = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AssignedToUserNames = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BackSrcActivityInstanceID = table.Column<int>(nullable: true),
                    BackwardType = table.Column<short>(nullable: false),
                    CanRenewInstance = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    CompareType = table.Column<short>(nullable: true),
                    CompleteOrder = table.Column<double>(nullable: true),
                    ComplexType = table.Column<short>(nullable: true),
                    CreatedByUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    EndedByUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    EndedByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EndedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GatewayDirectionTypeID = table.Column<short>(nullable: true),
                    LastUpdatedByUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LastUpdatedByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MIHostActivityInstanceID = table.Column<int>(nullable: true),
                    MergeType = table.Column<short>(nullable: true),
                    ProcessGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ProcessInstanceID = table.Column<int>(nullable: false),
                    RecordStatusInvalid = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    RowVersionID = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true),
                    SignForwardType = table.Column<short>(nullable: true),
                    TokensHad = table.Column<int>(nullable: false),
                    TokensRequired = table.Column<int>(nullable: false, defaultValue: 1),
                    WorkItemType = table.Column<short>(nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WfActivityInstance", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WfLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventTypeID = table.Column<int>(nullable: false),
                    InnerStackTrace = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    RequestData = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WfLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WfProcess",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EndExpression = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    EndType = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    IsUsing = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    LastUpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PageUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProcessGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ProcessName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartExpression = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StartType = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    Version = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "1"),
                    XmlContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XmlFileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    XmlFilePath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WfProcess", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WfProcessInstance",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppInstanceCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    AppInstanceID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedByUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    EndedByUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    EndedByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EndedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvokedActivityGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    InvokedActivityInstanceID = table.Column<int>(nullable: false, defaultValue: 0),
                    LastUpdatedByUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LastUpdatedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OverdueDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OverdueTreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParentProcessGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ParentProcessInstanceID = table.Column<int>(nullable: true, defaultValue: 0),
                    ProcessGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ProcessName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProcessState = table.Column<short>(nullable: false, defaultValue: (short)0),
                    RecordStatusInvalid = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    RowVersionID = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true),
                    Version = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WfProcessInstance", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WfTasks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActivityGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ActivityInstanceID = table.Column<int>(nullable: false),
                    ActivityName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    AppInstanceID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssignedToUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    AssignedToUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedByUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    EndedByUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    EndedByUserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    EndedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EntrustedTaskID = table.Column<int>(nullable: true),
                    LastUpdatedByUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LastUpdatedByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ProcessInstanceID = table.Column<int>(nullable: false),
                    RecordStatusInvalid = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    RowVersionID = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true),
                    TaskState = table.Column<short>(nullable: false, defaultValue: (short)0),
                    TaskType = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WfTasks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WfTransitionInstance",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppInstanceID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConditionParseResult = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    CreatedByUserID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedByUserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    FlyingType = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    FromActivityGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    FromActivityInstanceID = table.Column<int>(nullable: false),
                    FromActivityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromActivityType = table.Column<short>(nullable: false),
                    ProcessGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ProcessInstanceID = table.Column<int>(nullable: false),
                    RecordStatusInvalid = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    RowVersionID = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true),
                    ToActivityGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ToActivityInstanceID = table.Column<int>(nullable: false),
                    ToActivityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ToActivityType = table.Column<short>(nullable: false),
                    TransitionGUID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TransitionType = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WfTransitionInstance", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysDepartment");

            migrationBuilder.DropTable(
                name: "SysEmployee");

            migrationBuilder.DropTable(
                name: "SysEmployeeManager");

            migrationBuilder.DropTable(
                name: "SysRole");

            migrationBuilder.DropTable(
                name: "SysRoleUser");

            migrationBuilder.DropTable(
                name: "SysUser");

            migrationBuilder.DropTable(
                name: "WfActivityInstance");

            migrationBuilder.DropTable(
                name: "WfLog");

            migrationBuilder.DropTable(
                name: "WfProcess");

            migrationBuilder.DropTable(
                name: "WfProcessInstance");

            migrationBuilder.DropTable(
                name: "WfTasks");

            migrationBuilder.DropTable(
                name: "WfTransitionInstance");
        }
    }
}
