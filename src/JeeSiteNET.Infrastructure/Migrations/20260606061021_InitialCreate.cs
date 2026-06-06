using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JeeSiteNET.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sys_Config",
                columns: table => new
                {
                    ConfigKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConfigName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ConfigValue = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsSys = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Config", x => x.ConfigKey);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Dict_Data",
                columns: table => new
                {
                    DictCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DictType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DictLabel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DictValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Sort = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Dict_Data", x => x.DictCode);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Dict_Type",
                columns: table => new
                {
                    DictTypeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DictName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsSys = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Sort = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Dict_Type", x => x.DictTypeCode);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Log",
                columns: table => new
                {
                    LogId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LogType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LogTitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestUri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestMethod = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Params = table.Column<string>(type: "text", nullable: true),
                    DiffData = table.Column<string>(type: "text", nullable: true),
                    ExecuteTime = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    UserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OrgCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RemoteIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Log", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Menu",
                columns: table => new
                {
                    MenuCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MenuName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MenuHref = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MenuTarget = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MenuIcon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Permission = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    IsShow = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ModuleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ParentCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentCodes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreeSort = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TreeSorts = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreeLeaf = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    TreeLevel = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TreeNames = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Menu", x => x.MenuCode);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Module",
                columns: table => new
                {
                    ModuleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ModuleVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MainClass = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsEnabled = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Module", x => x.ModuleCode);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Organization",
                columns: table => new
                {
                    OrgCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrgName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrgType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrgTypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ParentCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentCodes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreeSort = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TreeSorts = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreeLeaf = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    TreeLevel = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TreeNames = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Organization", x => x.OrgCode);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Post",
                columns: table => new
                {
                    PostCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrgCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sort = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Post", x => x.PostCode);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Role",
                columns: table => new
                {
                    RoleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RoleType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsSys = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    UserType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sort = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Role", x => x.RoleCode);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Role_Menu",
                columns: table => new
                {
                    RoleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MenuCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Role_Menu", x => new { x.RoleCode, x.MenuCode });
                });

            migrationBuilder.CreateTable(
                name: "Sys_User",
                columns: table => new
                {
                    UserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LoginCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrgCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrgName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoginIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LoginCount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PwdUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_User", x => x.UserCode);
                });

            migrationBuilder.CreateTable(
                name: "Sys_User_Role",
                columns: table => new
                {
                    UserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_User_Role", x => new { x.UserCode, x.RoleCode });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Config_ConfigName",
                table: "Sys_Config",
                column: "ConfigName");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Dict_Data_DictType",
                table: "Sys_Dict_Data",
                column: "DictType");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Log_CreateDate",
                table: "Sys_Log",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Log_LogType",
                table: "Sys_Log",
                column: "LogType");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Menu_ParentCode",
                table: "Sys_Menu",
                column: "ParentCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Module_ModuleName",
                table: "Sys_Module",
                column: "ModuleName");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Organization_ParentCode",
                table: "Sys_Organization",
                column: "ParentCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Organization_ParentCodes",
                table: "Sys_Organization",
                column: "ParentCodes");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Organization_TreeSorts",
                table: "Sys_Organization",
                column: "TreeSorts");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Post_OrgCode",
                table: "Sys_Post",
                column: "OrgCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Post_Sort",
                table: "Sys_Post",
                column: "Sort");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Role_Menu_MenuCode",
                table: "Sys_Role_Menu",
                column: "MenuCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Role_Menu_RoleCode",
                table: "Sys_Role_Menu",
                column: "RoleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_User_LoginCode",
                table: "Sys_User",
                column: "LoginCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sys_User_OrgCode",
                table: "Sys_User",
                column: "OrgCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_User_UserType",
                table: "Sys_User",
                column: "UserType");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_User_Role_RoleCode",
                table: "Sys_User_Role",
                column: "RoleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_User_Role_UserCode",
                table: "Sys_User_Role",
                column: "UserCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sys_Config");

            migrationBuilder.DropTable(
                name: "Sys_Dict_Data");

            migrationBuilder.DropTable(
                name: "Sys_Dict_Type");

            migrationBuilder.DropTable(
                name: "Sys_Log");

            migrationBuilder.DropTable(
                name: "Sys_Menu");

            migrationBuilder.DropTable(
                name: "Sys_Module");

            migrationBuilder.DropTable(
                name: "Sys_Organization");

            migrationBuilder.DropTable(
                name: "Sys_Post");

            migrationBuilder.DropTable(
                name: "Sys_Role");

            migrationBuilder.DropTable(
                name: "Sys_Role_Menu");

            migrationBuilder.DropTable(
                name: "Sys_User");

            migrationBuilder.DropTable(
                name: "Sys_User_Role");
        }
    }
}
