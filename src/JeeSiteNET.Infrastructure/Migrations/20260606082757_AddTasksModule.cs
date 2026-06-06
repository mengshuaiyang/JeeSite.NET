using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JeeSiteNET.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTasksModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataScope",
                table: "Sys_Role",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bpm_ApprovalRecord",
                columns: table => new
                {
                    RecordId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkflowInstanceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BusinessKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BusinessType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActivityId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActivityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Assignee = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssigneeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Result = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bpm_ApprovalRecord", x => x.RecordId);
                });

            migrationBuilder.CreateTable(
                name: "Bpm_WorkflowForm",
                columns: table => new
                {
                    FormId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkflowInstanceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BusinessKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BusinessType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FormData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentActivityId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrentAssignee = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status1 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bpm_WorkflowForm", x => x.FormId);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Article",
                columns: table => new
                {
                    ArticleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Source = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsTop = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    IsRecommend = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    IsHot = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ClickCount = table.Column<long>(type: "bigint", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Article", x => x.ArticleCode);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Category",
                columns: table => new
                {
                    CategoryCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CategoryType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Link = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsShow = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ParentCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentCodes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreeSort = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TreeSorts = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TreeLeaf = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    TreeLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TreeNames = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Category", x => x.CategoryCode);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Site",
                columns: table => new
                {
                    SiteCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SiteName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Site", x => x.SiteCode);
                });

            migrationBuilder.CreateTable(
                name: "CodeGen_Table",
                columns: table => new
                {
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModuleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FunctionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FunctionAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TableComment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentTableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ParentFieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TplCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PackageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BusinessName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TreeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TreeParentCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TreeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Options = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGen_Table", x => x.TableName);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Tenant",
                columns: table => new
                {
                    TenantCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TenantName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpireDate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IsAvailable = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Tenant", x => x.TenantCode);
                });

            migrationBuilder.CreateTable(
                name: "Tasks_Job",
                columns: table => new
                {
                    JobId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    JobGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CronExpression = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AssemblyName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RunStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks_Job", x => x.JobId);
                });

            migrationBuilder.CreateTable(
                name: "Tasks_JobLog",
                columns: table => new
                {
                    LogId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    JobGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Result = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Duration = table.Column<long>(type: "bigint", nullable: true),
                    RunDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks_JobLog", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "CodeGen_TableColumn",
                columns: table => new
                {
                    ColumnId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ColumnName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ColumnSort = table.Column<int>(type: "int", nullable: true),
                    ColumnComment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ColumnType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NetType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    IsPk = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    IsNullable = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    IsInsert = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    IsEdit = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    IsList = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    IsQuery = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    QueryType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HtmlType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DictType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGen_TableColumn", x => x.ColumnId);
                    table.ForeignKey(
                        name: "FK_CodeGen_TableColumn_CodeGen_Table_TableName",
                        column: x => x.TableName,
                        principalTable: "CodeGen_Table",
                        principalColumn: "TableName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bpm_ApprovalRecord_Assignee",
                table: "Bpm_ApprovalRecord",
                column: "Assignee");

            migrationBuilder.CreateIndex(
                name: "IX_Bpm_ApprovalRecord_BusinessKey",
                table: "Bpm_ApprovalRecord",
                column: "BusinessKey");

            migrationBuilder.CreateIndex(
                name: "IX_Bpm_WorkflowForm_BusinessKey",
                table: "Bpm_WorkflowForm",
                column: "BusinessKey");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Article_CategoryCode",
                table: "Cms_Article",
                column: "CategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Article_PublishDate",
                table: "Cms_Article",
                column: "PublishDate");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGen_TableColumn_TableName",
                table: "CodeGen_TableColumn",
                column: "TableName");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_JobLog_JobId",
                table: "Tasks_JobLog",
                column: "JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bpm_ApprovalRecord");

            migrationBuilder.DropTable(
                name: "Bpm_WorkflowForm");

            migrationBuilder.DropTable(
                name: "Cms_Article");

            migrationBuilder.DropTable(
                name: "Cms_Category");

            migrationBuilder.DropTable(
                name: "Cms_Site");

            migrationBuilder.DropTable(
                name: "CodeGen_TableColumn");

            migrationBuilder.DropTable(
                name: "Sys_Tenant");

            migrationBuilder.DropTable(
                name: "Tasks_Job");

            migrationBuilder.DropTable(
                name: "Tasks_JobLog");

            migrationBuilder.DropTable(
                name: "CodeGen_Table");

            migrationBuilder.DropColumn(
                name: "DataScope",
                table: "Sys_Role");
        }
    }
}
