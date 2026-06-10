using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JeeSiteNET.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSysAuditAndEmpUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Cms_Article");

            migrationBuilder.RenameColumn(
                name: "Sort",
                table: "Sys_Role",
                newName: "RoleSort");

            migrationBuilder.RenameColumn(
                name: "Sort",
                table: "Sys_Post",
                newName: "PostSort");

            migrationBuilder.RenameIndex(
                name: "IX_Sys_Post_Sort",
                table: "Sys_Post",
                newName: "IX_Sys_Post_PostSort");

            migrationBuilder.RenameColumn(
                name: "OrgTypeName",
                table: "Sys_Organization",
                newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "CorpCode",
                table: "Sys_User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpName",
                table: "Sys_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD1",
                table: "Sys_User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD2",
                table: "Sys_User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD3",
                table: "Sys_User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD4",
                table: "Sys_User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF1",
                table: "Sys_User",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF2",
                table: "Sys_User",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF3",
                table: "Sys_User",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF4",
                table: "Sys_User",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI1",
                table: "Sys_User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI2",
                table: "Sys_User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI3",
                table: "Sys_User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI4",
                table: "Sys_User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendJson",
                table: "Sys_User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS1",
                table: "Sys_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS2",
                table: "Sys_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS3",
                table: "Sys_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS4",
                table: "Sys_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS5",
                table: "Sys_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS6",
                table: "Sys_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS7",
                table: "Sys_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS8",
                table: "Sys_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreezeCause",
                table: "Sys_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FreezeDate",
                table: "Sys_User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MgrType",
                table: "Sys_User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileImei",
                table: "Sys_User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PwdQuestion",
                table: "Sys_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PwdQuestionAnswer",
                table: "Sys_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PwdSecurityLevel",
                table: "Sys_User",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PwdUpdateRecord",
                table: "Sys_User",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefCode",
                table: "Sys_User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefName",
                table: "Sys_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Sys_User",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sign",
                table: "Sys_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UserWeight",
                table: "Sys_User",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WxOpenid",
                table: "Sys_User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BizScope",
                table: "Sys_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpCode",
                table: "Sys_Role",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpName",
                table: "Sys_Role",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesktopUrl",
                table: "Sys_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD1",
                table: "Sys_Role",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD2",
                table: "Sys_Role",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD3",
                table: "Sys_Role",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD4",
                table: "Sys_Role",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF1",
                table: "Sys_Role",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF2",
                table: "Sys_Role",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF3",
                table: "Sys_Role",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF4",
                table: "Sys_Role",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI1",
                table: "Sys_Role",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI2",
                table: "Sys_Role",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI3",
                table: "Sys_Role",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI4",
                table: "Sys_Role",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendJson",
                table: "Sys_Role",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS1",
                table: "Sys_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS2",
                table: "Sys_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS3",
                table: "Sys_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS4",
                table: "Sys_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS5",
                table: "Sys_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS6",
                table: "Sys_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS7",
                table: "Sys_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS8",
                table: "Sys_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsShow",
                table: "Sys_Role",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SysCodes",
                table: "Sys_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViewCode",
                table: "Sys_Role",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpCode",
                table: "Sys_Post",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpName",
                table: "Sys_Post",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostType",
                table: "Sys_Post",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViewCode",
                table: "Sys_Post",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Sys_Organization",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpCode",
                table: "Sys_Organization",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpName",
                table: "Sys_Organization",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Sys_Organization",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD1",
                table: "Sys_Organization",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD2",
                table: "Sys_Organization",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD3",
                table: "Sys_Organization",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD4",
                table: "Sys_Organization",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF1",
                table: "Sys_Organization",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF2",
                table: "Sys_Organization",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF3",
                table: "Sys_Organization",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF4",
                table: "Sys_Organization",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI1",
                table: "Sys_Organization",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI2",
                table: "Sys_Organization",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI3",
                table: "Sys_Organization",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI4",
                table: "Sys_Organization",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendJson",
                table: "Sys_Organization",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS1",
                table: "Sys_Organization",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS2",
                table: "Sys_Organization",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS3",
                table: "Sys_Organization",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS4",
                table: "Sys_Organization",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS5",
                table: "Sys_Organization",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS6",
                table: "Sys_Organization",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS7",
                table: "Sys_Organization",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS8",
                table: "Sys_Organization",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Leader",
                table: "Sys_Organization",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Sys_Organization",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViewCode",
                table: "Sys_Organization",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Sys_Organization",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentVersion",
                table: "Sys_Module",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sys_Module",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GenBaseDir",
                table: "Sys_Module",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GenFrontDir",
                table: "Sys_Module",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ModuleSort",
                table: "Sys_Module",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageName",
                table: "Sys_Module",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TplCategory",
                table: "Sys_Module",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpgradeInfo",
                table: "Sys_Module",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Component",
                table: "Sys_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpCode",
                table: "Sys_Menu",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpName",
                table: "Sys_Menu",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD1",
                table: "Sys_Menu",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD2",
                table: "Sys_Menu",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD3",
                table: "Sys_Menu",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD4",
                table: "Sys_Menu",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF1",
                table: "Sys_Menu",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF2",
                table: "Sys_Menu",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF3",
                table: "Sys_Menu",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF4",
                table: "Sys_Menu",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI1",
                table: "Sys_Menu",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI2",
                table: "Sys_Menu",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI3",
                table: "Sys_Menu",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI4",
                table: "Sys_Menu",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendJson",
                table: "Sys_Menu",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS1",
                table: "Sys_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS2",
                table: "Sys_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS3",
                table: "Sys_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS4",
                table: "Sys_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS5",
                table: "Sys_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS6",
                table: "Sys_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS7",
                table: "Sys_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS8",
                table: "Sys_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuColor",
                table: "Sys_Menu",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuTitle",
                table: "Sys_Menu",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuType",
                table: "Sys_Menu",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModuleCodes",
                table: "Sys_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Params",
                table: "Sys_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SysCode",
                table: "Sys_Menu",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BizKey",
                table: "Sys_Log",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BizType",
                table: "Sys_Log",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrowserName",
                table: "Sys_Log",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpCode",
                table: "Sys_Log",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpName",
                table: "Sys_Log",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateByName",
                table: "Sys_Log",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Sys_Log",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExceptionInfo",
                table: "Sys_Log",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsException",
                table: "Sys_Log",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServerAddr",
                table: "Sys_Log",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpCode",
                table: "Sys_Dict_Data",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpName",
                table: "Sys_Dict_Data",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CssClass",
                table: "Sys_Dict_Data",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CssStyle",
                table: "Sys_Dict_Data",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sys_Dict_Data",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DictIcon",
                table: "Sys_Dict_Data",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD1",
                table: "Sys_Dict_Data",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD2",
                table: "Sys_Dict_Data",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD3",
                table: "Sys_Dict_Data",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD4",
                table: "Sys_Dict_Data",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF1",
                table: "Sys_Dict_Data",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF2",
                table: "Sys_Dict_Data",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF3",
                table: "Sys_Dict_Data",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF4",
                table: "Sys_Dict_Data",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI1",
                table: "Sys_Dict_Data",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI2",
                table: "Sys_Dict_Data",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI3",
                table: "Sys_Dict_Data",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI4",
                table: "Sys_Dict_Data",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendJson",
                table: "Sys_Dict_Data",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS1",
                table: "Sys_Dict_Data",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS2",
                table: "Sys_Dict_Data",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS3",
                table: "Sys_Dict_Data",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS4",
                table: "Sys_Dict_Data",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS5",
                table: "Sys_Dict_Data",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS6",
                table: "Sys_Dict_Data",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS7",
                table: "Sys_Dict_Data",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS8",
                table: "Sys_Dict_Data",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentCode",
                table: "Sys_Dict_Data",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParentCodes",
                table: "Sys_Dict_Data",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TreeLeaf",
                table: "Sys_Dict_Data",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TreeLevel",
                table: "Sys_Dict_Data",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TreeNames",
                table: "Sys_Dict_Data",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TreeSort",
                table: "Sys_Dict_Data",
                type: "decimal(18,2)",
                maxLength: 2000,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TreeSorts",
                table: "Sys_Dict_Data",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Copyright",
                table: "Cms_Site",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpCode",
                table: "Cms_Site",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpName",
                table: "Cms_Site",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomIndexView",
                table: "Cms_Site",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SiteSort",
                table: "Cms_Site",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "Cms_Site",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Cms_Site",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TreeSort",
                table: "Cms_Category",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TreeLevel",
                table: "Cms_Category",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "CorpCode",
                table: "Cms_Category",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpName",
                table: "Cms_Category",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomContentView",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomListView",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD1",
                table: "Cms_Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD2",
                table: "Cms_Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD3",
                table: "Cms_Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendD4",
                table: "Cms_Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF1",
                table: "Cms_Category",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF2",
                table: "Cms_Category",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF3",
                table: "Cms_Category",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendF4",
                table: "Cms_Category",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI1",
                table: "Cms_Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI2",
                table: "Cms_Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI3",
                table: "Cms_Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendI4",
                table: "Cms_Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendJson",
                table: "Cms_Category",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS1",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS2",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS3",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS4",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS5",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS6",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS7",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendS8",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InList",
                table: "Cms_Category",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InMenu",
                table: "Cms_Category",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsCanComment",
                table: "Cms_Category",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsNeedAudit",
                table: "Cms_Category",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShowModes",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Target",
                table: "Cms_Category",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViewConfig",
                table: "Cms_Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Cms_Article",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Copyfrom",
                table: "Cms_Article",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpCode",
                table: "Cms_Article",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorpName",
                table: "Cms_Article",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomContentView",
                table: "Cms_Article",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Cms_Article",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "HitsMinus",
                table: "Cms_Article",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "HitsPlus",
                table: "Cms_Article",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Cms_Article",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModuleType",
                table: "Cms_Article",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViewConfig",
                table: "Cms_Article",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Cms_Article",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WeightDate",
                table: "Cms_Article",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WordCount",
                table: "Cms_Article",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Biz_Category",
                columns: table => new
                {
                    CategoryCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ViewCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CorpCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorpName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ParentCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentCodes = table.Column<string>(type: "nvarchar(767)", maxLength: 767, nullable: false),
                    TreeSort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TreeSorts = table.Column<string>(type: "nvarchar(767)", maxLength: 767, nullable: false),
                    TreeLeaf = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    TreeLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TreeNames = table.Column<string>(type: "nvarchar(767)", maxLength: 767, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biz_Category", x => x.CategoryCode);
                });

            migrationBuilder.CreateTable(
                name: "Bpm_LeaveRequest",
                columns: table => new
                {
                    LeaveRequestId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Applicant = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    LeaveType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationDays = table.Column<double>(type: "float", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ManagerApprover = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    HrApprover = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SubmitDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bpm_LeaveRequest", x => x.LeaveRequestId);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Article_Data",
                columns: table => new
                {
                    ArticleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Relation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsCanComment = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ExtendS1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExtendS2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExtendS3 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExtendS4 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExtendS5 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExtendS6 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExtendS7 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExtendS8 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExtendI1 = table.Column<int>(type: "int", nullable: true),
                    ExtendI2 = table.Column<int>(type: "int", nullable: true),
                    ExtendI3 = table.Column<int>(type: "int", nullable: true),
                    ExtendI4 = table.Column<int>(type: "int", nullable: true),
                    ExtendF1 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ExtendF2 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ExtendF3 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ExtendF4 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ExtendD1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendD2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendD3 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendD4 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Article_Data", x => x.ArticleCode);
                    table.ForeignKey(
                        name: "FK_Cms_Article_Data_Cms_Article_ArticleCode",
                        column: x => x.ArticleCode,
                        principalTable: "Cms_Article",
                        principalColumn: "ArticleCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Article_PosId",
                columns: table => new
                {
                    ArticleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PosId = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Article_PosId", x => new { x.ArticleCode, x.PosId });
                    table.ForeignKey(
                        name: "FK_Cms_Article_PosId_Cms_Article_ArticleCode",
                        column: x => x.ArticleCode,
                        principalTable: "Cms_Article",
                        principalColumn: "ArticleCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Comment",
                columns: table => new
                {
                    CommentCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ArticleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ArticleTitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Ip = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AuditUserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuditComment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HitsPlus = table.Column<long>(type: "bigint", nullable: true),
                    HitsMinus = table.Column<long>(type: "bigint", nullable: true),
                    CorpCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorpName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Comment", x => x.CommentCode);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Guestbook",
                columns: table => new
                {
                    GbCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GbType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReUserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReContent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CorpCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorpName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Guestbook", x => x.GbCode);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Tag",
                columns: table => new
                {
                    TagName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ClickNum = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Tag", x => x.TagName);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Visit_Log",
                columns: table => new
                {
                    VisitId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RequestUrlHost = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SourceReferer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SourceRefererHost = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SourceType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SearchEngine = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SearchWord = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RemoteAddr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserLanguage = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    UserScreenSize = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    UserDevice = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    UserOsName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    UserBrowser = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    UserBrowserVersion = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    UniqueVisitId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VisitDate = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    VisitTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsNewVisit = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    FirstVisitTime = table.Column<long>(type: "bigint", nullable: true),
                    PrevRemainTime = table.Column<long>(type: "bigint", nullable: true),
                    TotalRemainTime = table.Column<long>(type: "bigint", nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CategoryCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContentId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContentTitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VisitUserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VisitUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorpCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorpName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Visit_Log", x => x.VisitId);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Area",
                columns: table => new
                {
                    AreaCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AreaType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ParentCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentCodes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreeSort = table.Column<decimal>(type: "decimal(18,2)", maxLength: 2000, nullable: false),
                    TreeSorts = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreeLeaf = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    TreeLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TreeNames = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Area", x => x.AreaCode);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Audit",
                columns: table => new
                {
                    AuditId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AuditType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AuditResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LoginCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OfficeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OfficeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MenuCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PwdSecurityLevel = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PwdUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Audit", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Company",
                columns: table => new
                {
                    CompanyCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ViewCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AreaCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AreaName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CorpCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorpName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExtendS1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendI1 = table.Column<int>(type: "int", nullable: true),
                    ExtendI2 = table.Column<int>(type: "int", nullable: true),
                    ExtendI3 = table.Column<int>(type: "int", nullable: true),
                    ExtendI4 = table.Column<int>(type: "int", nullable: true),
                    ExtendF1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtendF2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtendF3 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtendF4 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtendD1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendD2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendD3 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendD4 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendJson = table.Column<string>(type: "text", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ParentCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentCodes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreeSort = table.Column<decimal>(type: "decimal(18,2)", maxLength: 2000, nullable: false),
                    TreeSorts = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreeLeaf = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    TreeLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TreeNames = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Company", x => x.CompanyCode);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Company_Office",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OfficeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Company_Office", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Employee",
                columns: table => new
                {
                    EmpCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmpNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmpName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmpNameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OfficeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OfficeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CorpCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorpName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExtendS1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendS8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendI1 = table.Column<int>(type: "int", nullable: true),
                    ExtendI2 = table.Column<int>(type: "int", nullable: true),
                    ExtendI3 = table.Column<int>(type: "int", nullable: true),
                    ExtendI4 = table.Column<int>(type: "int", nullable: true),
                    ExtendF1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtendF2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtendF3 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtendF4 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtendD1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendD2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendD3 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendD4 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendJson = table.Column<string>(type: "text", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Employee", x => x.EmpCode);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Employee_Office",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmpCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OfficeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Employee_Office", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Employee_Post",
                columns: table => new
                {
                    EmpCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Employee_Post", x => new { x.EmpCode, x.PostCode });
                });

            migrationBuilder.CreateTable(
                name: "Sys_EmpUser",
                columns: table => new
                {
                    EmpCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmpName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LoginCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_EmpUser", x => new { x.EmpCode, x.UserCode });
                });

            migrationBuilder.CreateTable(
                name: "Sys_File_Entity",
                columns: table => new
                {
                    FileId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileMd5 = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FileContentType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<decimal>(type: "decimal(31,0)", nullable: false),
                    FileMeta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FilePreview = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_File_Entity", x => x.FileId);
                });

            migrationBuilder.CreateTable(
                name: "Sys_File_Upload",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FileSort = table.Column<decimal>(type: "decimal(10,0)", nullable: true),
                    BizKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BizType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_File_Upload", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Lang",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LangCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LangText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LangType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Lang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Menu_Data_Scope",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MenuCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RuleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RuleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RuleConfig = table.Column<string>(type: "text", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Menu_Data_Scope", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Msg_Inner",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MsgTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContentLevel = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    MsgContent = table.Column<string>(type: "text", nullable: false),
                    ReceiveType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ReceiveCodes = table.Column<string>(type: "text", nullable: true),
                    ReceiveNames = table.Column<string>(type: "text", nullable: true),
                    SendUserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SendUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAttac = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    NotifyTypes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Msg_Inner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Msg_Inner_Record",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MsgInnerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiveUserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiveUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReadStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsStar = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Msg_Inner_Record", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Msg_Push",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MsgType = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    MsgTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MsgContent = table.Column<string>(type: "text", nullable: false),
                    BizKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BizType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceiveCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiveUserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiveUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SendUserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SendUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsMergePush = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PlanPushDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PushNumber = table.Column<int>(type: "int", nullable: true),
                    PushReturnCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PushReturnMsgId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PushReturnContent = table.Column<string>(type: "text", nullable: true),
                    PushStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PushDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Msg_Push", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Msg_Pushed",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MsgType = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    MsgTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MsgContent = table.Column<string>(type: "text", nullable: false),
                    BizKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BizType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceiveCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiveUserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiveUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SendUserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SendUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsMergePush = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PlanPushDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PushNumber = table.Column<int>(type: "int", nullable: true),
                    PushReturnCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PushReturnMsgId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PushReturnContent = table.Column<string>(type: "text", nullable: true),
                    PushStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PushDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Msg_Pushed", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Msg_Template",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModuleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TplKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TplName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TplType = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    TplContent = table.Column<string>(type: "text", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Msg_Template", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Post_Role",
                columns: table => new
                {
                    PostCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Post_Role", x => new { x.PostCode, x.RoleCode });
                });

            migrationBuilder.CreateTable(
                name: "Sys_Role_Data_Scope",
                columns: table => new
                {
                    RoleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MenuCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RuleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RuleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RuleConfig = table.Column<string>(type: "text", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Role_Data_Scope", x => new { x.RoleCode, x.MenuCode });
                });

            migrationBuilder.CreateTable(
                name: "Sys_Role_Field_Scope",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MenuCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EntityLabel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EntityClass = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FieldConfig = table.Column<string>(type: "text", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Role_Field_Scope", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_User_Data_Scope",
                columns: table => new
                {
                    UserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CtrlType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CtrlData = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CtrlPermi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_User_Data_Scope", x => x.UserCode);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Article_Tag",
                columns: table => new
                {
                    ArticleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TagName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Article_Tag", x => new { x.ArticleCode, x.TagName });
                    table.ForeignKey(
                        name: "FK_Cms_Article_Tag_Cms_Article_ArticleCode",
                        column: x => x.ArticleCode,
                        principalTable: "Cms_Article",
                        principalColumn: "ArticleCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cms_Article_Tag_Cms_Tag_TagName",
                        column: x => x.TagName,
                        principalTable: "Cms_Tag",
                        principalColumn: "TagName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Dict_Data_ParentCode",
                table: "Sys_Dict_Data",
                column: "ParentCode");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGen_Table_ModuleCode",
                table: "CodeGen_Table",
                column: "ModuleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Category_ParentCode",
                table: "Cms_Category",
                column: "ParentCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Category_SiteCode",
                table: "Cms_Category",
                column: "SiteCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Article_IsTop_PublishDate",
                table: "Cms_Article",
                columns: new[] { "IsTop", "PublishDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Article_Status",
                table: "Cms_Article",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Biz_Category_ParentCode",
                table: "Biz_Category",
                column: "ParentCode");

            migrationBuilder.CreateIndex(
                name: "IX_Biz_Category_TreeSort",
                table: "Biz_Category",
                column: "TreeSort");

            migrationBuilder.CreateIndex(
                name: "IX_Biz_Category_ViewCode",
                table: "Biz_Category",
                column: "ViewCode");

            migrationBuilder.CreateIndex(
                name: "IX_Bpm_LeaveRequest_Applicant",
                table: "Bpm_LeaveRequest",
                column: "Applicant");

            migrationBuilder.CreateIndex(
                name: "IX_Bpm_LeaveRequest_Status",
                table: "Bpm_LeaveRequest",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Article_PosId_ArticleCode",
                table: "Cms_Article_PosId",
                column: "ArticleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Article_Tag_ArticleCode",
                table: "Cms_Article_Tag",
                column: "ArticleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Article_Tag_TagName",
                table: "Cms_Article_Tag",
                column: "TagName");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Comment_ArticleCode",
                table: "Cms_Comment",
                column: "ArticleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Comment_CategoryCode",
                table: "Cms_Comment",
                column: "CategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Guestbook_GbType",
                table: "Cms_Guestbook",
                column: "GbType");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Visit_Log_CategoryCode",
                table: "Cms_Visit_Log",
                column: "CategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Visit_Log_ContentId",
                table: "Cms_Visit_Log",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Visit_Log_VisitDate",
                table: "Cms_Visit_Log",
                column: "VisitDate");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Area_AreaType",
                table: "Sys_Area",
                column: "AreaType");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Audit_AuditType",
                table: "Sys_Audit",
                column: "AuditType");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Audit_CreateDate",
                table: "Sys_Audit",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Audit_UserCode",
                table: "Sys_Audit",
                column: "UserCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Company_AreaCode",
                table: "Sys_Company",
                column: "AreaCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Company_ViewCode",
                table: "Sys_Company",
                column: "ViewCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Company_Office_CompanyCode",
                table: "Sys_Company_Office",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Company_Office_OfficeCode",
                table: "Sys_Company_Office",
                column: "OfficeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Employee_CompanyCode",
                table: "Sys_Employee",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Employee_EmpNo",
                table: "Sys_Employee",
                column: "EmpNo");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Employee_OfficeCode",
                table: "Sys_Employee",
                column: "OfficeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Employee_Office_EmpCode",
                table: "Sys_Employee_Office",
                column: "EmpCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Employee_Office_OfficeCode",
                table: "Sys_Employee_Office",
                column: "OfficeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Employee_Post_EmpCode",
                table: "Sys_Employee_Post",
                column: "EmpCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Employee_Post_PostCode",
                table: "Sys_Employee_Post",
                column: "PostCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_EmpUser_EmpCode",
                table: "Sys_EmpUser",
                column: "EmpCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_EmpUser_UserCode",
                table: "Sys_EmpUser",
                column: "UserCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sys_File_Entity_FileMd5",
                table: "Sys_File_Entity",
                column: "FileMd5");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_File_Upload_BizKey",
                table: "Sys_File_Upload",
                column: "BizKey");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_File_Upload_BizType",
                table: "Sys_File_Upload",
                column: "BizType");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_File_Upload_CreateBy",
                table: "Sys_File_Upload",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_File_Upload_FileId",
                table: "Sys_File_Upload",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Lang_LangCode_LangType",
                table: "Sys_Lang",
                columns: new[] { "LangCode", "LangType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Menu_Data_Scope_RoleCode",
                table: "Sys_Menu_Data_Scope",
                column: "RoleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Inner_CreateBy",
                table: "Sys_Msg_Inner",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Inner_SendDate",
                table: "Sys_Msg_Inner",
                column: "SendDate");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Inner_SendUserCode",
                table: "Sys_Msg_Inner",
                column: "SendUserCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Inner_Record_MsgInnerId",
                table: "Sys_Msg_Inner_Record",
                column: "MsgInnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Inner_Record_ReadStatus",
                table: "Sys_Msg_Inner_Record",
                column: "ReadStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Inner_Record_ReceiveUserCode",
                table: "Sys_Msg_Inner_Record",
                column: "ReceiveUserCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Push_BizType_BizKey",
                table: "Sys_Msg_Push",
                columns: new[] { "BizType", "BizKey" });

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Push_IsMergePush",
                table: "Sys_Msg_Push",
                column: "IsMergePush");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Push_MsgType",
                table: "Sys_Msg_Push",
                column: "MsgType");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Push_PushStatus",
                table: "Sys_Msg_Push",
                column: "PushStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Push_ReadStatus",
                table: "Sys_Msg_Push",
                column: "ReadStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Push_ReceiveCode",
                table: "Sys_Msg_Push",
                column: "ReceiveCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Push_ReceiveUserCode",
                table: "Sys_Msg_Push",
                column: "ReceiveUserCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Pushed_BizType_BizKey",
                table: "Sys_Msg_Pushed",
                columns: new[] { "BizType", "BizKey" });

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Pushed_IsMergePush",
                table: "Sys_Msg_Pushed",
                column: "IsMergePush");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Pushed_MsgType",
                table: "Sys_Msg_Pushed",
                column: "MsgType");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Pushed_PushStatus",
                table: "Sys_Msg_Pushed",
                column: "PushStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Pushed_ReadStatus",
                table: "Sys_Msg_Pushed",
                column: "ReadStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Pushed_ReceiveCode",
                table: "Sys_Msg_Pushed",
                column: "ReceiveCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Pushed_ReceiveUserCode",
                table: "Sys_Msg_Pushed",
                column: "ReceiveUserCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Template_Status",
                table: "Sys_Msg_Template",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Template_TplKey",
                table: "Sys_Msg_Template",
                column: "TplKey");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Msg_Template_TplType",
                table: "Sys_Msg_Template",
                column: "TplType");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Post_Role_PostCode",
                table: "Sys_Post_Role",
                column: "PostCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Post_Role_RoleCode",
                table: "Sys_Post_Role",
                column: "RoleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Role_Data_Scope_RoleCode",
                table: "Sys_Role_Data_Scope",
                column: "RoleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Role_Field_Scope_RoleCode",
                table: "Sys_Role_Field_Scope",
                column: "RoleCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biz_Category");

            migrationBuilder.DropTable(
                name: "Bpm_LeaveRequest");

            migrationBuilder.DropTable(
                name: "Cms_Article_Data");

            migrationBuilder.DropTable(
                name: "Cms_Article_PosId");

            migrationBuilder.DropTable(
                name: "Cms_Article_Tag");

            migrationBuilder.DropTable(
                name: "Cms_Comment");

            migrationBuilder.DropTable(
                name: "Cms_Guestbook");

            migrationBuilder.DropTable(
                name: "Cms_Visit_Log");

            migrationBuilder.DropTable(
                name: "Sys_Area");

            migrationBuilder.DropTable(
                name: "Sys_Audit");

            migrationBuilder.DropTable(
                name: "Sys_Company");

            migrationBuilder.DropTable(
                name: "Sys_Company_Office");

            migrationBuilder.DropTable(
                name: "Sys_Employee");

            migrationBuilder.DropTable(
                name: "Sys_Employee_Office");

            migrationBuilder.DropTable(
                name: "Sys_Employee_Post");

            migrationBuilder.DropTable(
                name: "Sys_EmpUser");

            migrationBuilder.DropTable(
                name: "Sys_File_Entity");

            migrationBuilder.DropTable(
                name: "Sys_File_Upload");

            migrationBuilder.DropTable(
                name: "Sys_Lang");

            migrationBuilder.DropTable(
                name: "Sys_Menu_Data_Scope");

            migrationBuilder.DropTable(
                name: "Sys_Msg_Inner");

            migrationBuilder.DropTable(
                name: "Sys_Msg_Inner_Record");

            migrationBuilder.DropTable(
                name: "Sys_Msg_Push");

            migrationBuilder.DropTable(
                name: "Sys_Msg_Pushed");

            migrationBuilder.DropTable(
                name: "Sys_Msg_Template");

            migrationBuilder.DropTable(
                name: "Sys_Post_Role");

            migrationBuilder.DropTable(
                name: "Sys_Role_Data_Scope");

            migrationBuilder.DropTable(
                name: "Sys_Role_Field_Scope");

            migrationBuilder.DropTable(
                name: "Sys_User_Data_Scope");

            migrationBuilder.DropTable(
                name: "Cms_Tag");

            migrationBuilder.DropIndex(
                name: "IX_Sys_Dict_Data_ParentCode",
                table: "Sys_Dict_Data");

            migrationBuilder.DropIndex(
                name: "IX_CodeGen_Table_ModuleCode",
                table: "CodeGen_Table");

            migrationBuilder.DropIndex(
                name: "IX_Cms_Category_ParentCode",
                table: "Cms_Category");

            migrationBuilder.DropIndex(
                name: "IX_Cms_Category_SiteCode",
                table: "Cms_Category");

            migrationBuilder.DropIndex(
                name: "IX_Cms_Article_IsTop_PublishDate",
                table: "Cms_Article");

            migrationBuilder.DropIndex(
                name: "IX_Cms_Article_Status",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "CorpCode",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "CorpName",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendD1",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendD2",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendD3",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendD4",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendF1",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendF2",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendF3",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendF4",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendI1",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendI2",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendI3",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendI4",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendJson",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendS1",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendS2",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendS3",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendS4",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendS5",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendS6",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendS7",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "ExtendS8",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "FreezeCause",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "FreezeDate",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "MgrType",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "MobileImei",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "PwdQuestion",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "PwdQuestionAnswer",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "PwdSecurityLevel",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "PwdUpdateRecord",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "RefCode",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "RefName",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "Sign",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "UserWeight",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "WxOpenid",
                table: "Sys_User");

            migrationBuilder.DropColumn(
                name: "BizScope",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "CorpCode",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "CorpName",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "DesktopUrl",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendD1",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendD2",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendD3",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendD4",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendF1",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendF2",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendF3",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendF4",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendI1",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendI2",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendI3",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendI4",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendJson",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendS1",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendS2",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendS3",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendS4",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendS5",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendS6",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendS7",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ExtendS8",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "IsShow",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "SysCodes",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "ViewCode",
                table: "Sys_Role");

            migrationBuilder.DropColumn(
                name: "CorpCode",
                table: "Sys_Post");

            migrationBuilder.DropColumn(
                name: "CorpName",
                table: "Sys_Post");

            migrationBuilder.DropColumn(
                name: "PostType",
                table: "Sys_Post");

            migrationBuilder.DropColumn(
                name: "ViewCode",
                table: "Sys_Post");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "CorpCode",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "CorpName",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendD1",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendD2",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendD3",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendD4",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendF1",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendF2",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendF3",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendF4",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendI1",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendI2",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendI3",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendI4",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendJson",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendS1",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendS2",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendS3",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendS4",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendS5",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendS6",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendS7",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ExtendS8",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "Leader",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ViewCode",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Sys_Organization");

            migrationBuilder.DropColumn(
                name: "CurrentVersion",
                table: "Sys_Module");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sys_Module");

            migrationBuilder.DropColumn(
                name: "GenBaseDir",
                table: "Sys_Module");

            migrationBuilder.DropColumn(
                name: "GenFrontDir",
                table: "Sys_Module");

            migrationBuilder.DropColumn(
                name: "ModuleSort",
                table: "Sys_Module");

            migrationBuilder.DropColumn(
                name: "PackageName",
                table: "Sys_Module");

            migrationBuilder.DropColumn(
                name: "TplCategory",
                table: "Sys_Module");

            migrationBuilder.DropColumn(
                name: "UpgradeInfo",
                table: "Sys_Module");

            migrationBuilder.DropColumn(
                name: "Component",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "CorpCode",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "CorpName",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendD1",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendD2",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendD3",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendD4",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendF1",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendF2",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendF3",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendF4",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendI1",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendI2",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendI3",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendI4",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendJson",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendS1",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendS2",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendS3",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendS4",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendS5",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendS6",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendS7",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ExtendS8",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "MenuColor",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "MenuTitle",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "MenuType",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "ModuleCodes",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "Params",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "SysCode",
                table: "Sys_Menu");

            migrationBuilder.DropColumn(
                name: "BizKey",
                table: "Sys_Log");

            migrationBuilder.DropColumn(
                name: "BizType",
                table: "Sys_Log");

            migrationBuilder.DropColumn(
                name: "BrowserName",
                table: "Sys_Log");

            migrationBuilder.DropColumn(
                name: "CorpCode",
                table: "Sys_Log");

            migrationBuilder.DropColumn(
                name: "CorpName",
                table: "Sys_Log");

            migrationBuilder.DropColumn(
                name: "CreateByName",
                table: "Sys_Log");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Sys_Log");

            migrationBuilder.DropColumn(
                name: "ExceptionInfo",
                table: "Sys_Log");

            migrationBuilder.DropColumn(
                name: "IsException",
                table: "Sys_Log");

            migrationBuilder.DropColumn(
                name: "ServerAddr",
                table: "Sys_Log");

            migrationBuilder.DropColumn(
                name: "CorpCode",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "CorpName",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "CssClass",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "CssStyle",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "DictIcon",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendD1",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendD2",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendD3",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendD4",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendF1",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendF2",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendF3",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendF4",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendI1",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendI2",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendI3",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendI4",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendJson",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendS1",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendS2",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendS3",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendS4",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendS5",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendS6",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendS7",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ExtendS8",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ParentCode",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "ParentCodes",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "TreeLeaf",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "TreeLevel",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "TreeNames",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "TreeSort",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "TreeSorts",
                table: "Sys_Dict_Data");

            migrationBuilder.DropColumn(
                name: "Copyright",
                table: "Cms_Site");

            migrationBuilder.DropColumn(
                name: "CorpCode",
                table: "Cms_Site");

            migrationBuilder.DropColumn(
                name: "CorpName",
                table: "Cms_Site");

            migrationBuilder.DropColumn(
                name: "CustomIndexView",
                table: "Cms_Site");

            migrationBuilder.DropColumn(
                name: "SiteSort",
                table: "Cms_Site");

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "Cms_Site");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Cms_Site");

            migrationBuilder.DropColumn(
                name: "CorpCode",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "CorpName",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "CustomContentView",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "CustomListView",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendD1",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendD2",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendD3",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendD4",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendF1",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendF2",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendF3",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendF4",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendI1",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendI2",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendI3",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendI4",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendJson",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendS1",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendS2",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendS3",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendS4",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendS5",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendS6",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendS7",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ExtendS8",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "InList",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "InMenu",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "IsCanComment",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "IsNeedAudit",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ShowModes",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "Target",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "ViewConfig",
                table: "Cms_Category");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "Copyfrom",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "CorpCode",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "CorpName",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "CustomContentView",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "HitsMinus",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "HitsPlus",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "ModuleType",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "ViewConfig",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "WeightDate",
                table: "Cms_Article");

            migrationBuilder.DropColumn(
                name: "WordCount",
                table: "Cms_Article");

            migrationBuilder.RenameColumn(
                name: "RoleSort",
                table: "Sys_Role",
                newName: "Sort");

            migrationBuilder.RenameColumn(
                name: "PostSort",
                table: "Sys_Post",
                newName: "Sort");

            migrationBuilder.RenameIndex(
                name: "IX_Sys_Post_PostSort",
                table: "Sys_Post",
                newName: "IX_Sys_Post_Sort");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Sys_Organization",
                newName: "OrgTypeName");

            migrationBuilder.AlterColumn<decimal>(
                name: "TreeSort",
                table: "Cms_Category",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TreeLevel",
                table: "Cms_Category",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Cms_Article",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
