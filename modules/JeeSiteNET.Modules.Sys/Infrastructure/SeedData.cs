using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Modules.Sys.Infrastructure;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<JeeSiteDbContext>();
        await db.Database.EnsureCreatedAsync();

        if (!await db.Set<User>().AnyAsync())
        {
            var now = DateTime.Now;
            var adminUserCode = IdGenerator.NewId();

            var adminRole = new Role
            {
                RoleCode = IdGenerator.NewId(),
                RoleName = "系统管理员",
                RoleType = "admin",
                IsSys = "1",
                RoleSort = 1,
                DataScope = "All",
                Status = "0",
                CreateDate = now,
                UpdateDate = now
            };
            db.Set<Role>().Add(adminRole);

            var generalRole = new Role
            {
                RoleCode = IdGenerator.NewId(),
                RoleName = "普通用户",
                RoleType = "user",
                IsSys = "1",
                RoleSort = 2,
                DataScope = "CompanyAndChild",
                Status = "0",
                CreateDate = now,
                UpdateDate = now
            };
            db.Set<Role>().Add(generalRole);

            var admin = new User
            {
                UserCode = adminUserCode,
                LoginCode = "admin",
                UserName = "系统管理员",
                Password = "21232f297a57a5a743894a0e4a801fc3",
                UserType = "employee",
                Status = "0",
                CreateDate = now,
                UpdateDate = now
            };
            db.Set<User>().Add(admin);

            var sysMenu = new Menu
            {
                MenuCode = IdGenerator.NewId(),
                MenuName = "系统管理",
                MenuIcon = "setting",
                Permission = "sys",
                IsShow = "1",
                ParentCode = "0",
                TreeSort = 1,
                ModuleCode = "Sys",
                Status = "0",
                CreateDate = now,
                UpdateDate = now
            };
            db.Set<Menu>().Add(sysMenu);

            var userMenu = new Menu
            {
                MenuCode = IdGenerator.NewId(),
                MenuName = "用户管理",
                MenuHref = "/sys/user",
                MenuIcon = "user",
                Permission = "sys:user",
                IsShow = "1",
                ParentCode = sysMenu.MenuCode,
                TreeSort = 10,
                ModuleCode = "Sys",
                Status = "0",
                CreateDate = now,
                UpdateDate = now
            };
            db.Set<Menu>().Add(userMenu);

            var roleMenu = new Menu
            {
                MenuCode = IdGenerator.NewId(),
                MenuName = "角色管理",
                MenuHref = "/sys/role",
                MenuIcon = "team",
                Permission = "sys:role",
                IsShow = "1",
                ParentCode = sysMenu.MenuCode,
                TreeSort = 20,
                ModuleCode = "Sys",
                Status = "0",
                CreateDate = now,
                UpdateDate = now
            };
            db.Set<Menu>().Add(roleMenu);

            var menuMenu = new Menu
            {
                MenuCode = IdGenerator.NewId(),
                MenuName = "菜单管理",
                MenuHref = "/sys/menu",
                MenuIcon = "menu",
                Permission = "sys:menu",
                IsShow = "1",
                ParentCode = sysMenu.MenuCode,
                TreeSort = 30,
                ModuleCode = "Sys",
                Status = "0",
                CreateDate = now,
                UpdateDate = now
            };
            db.Set<Menu>().Add(menuMenu);

            var orgMenu = new Menu
            {
                MenuCode = IdGenerator.NewId(),
                MenuName = "机构管理",
                MenuHref = "/sys/org",
                MenuIcon = "apartment",
                Permission = "sys:org",
                IsShow = "1",
                ParentCode = sysMenu.MenuCode,
                TreeSort = 40,
                ModuleCode = "Sys",
                Status = "0",
                CreateDate = now,
                UpdateDate = now
            };
            db.Set<Menu>().Add(orgMenu);

            var rootOrg = new Organization
            {
                OrgCode = IdGenerator.NewId(),
                OrgName = "JeeSite.NET",
                OrgType = "company",
                ParentCode = "0",
                TreeSort = 1,
                Status = "0",
                CreateDate = now,
                UpdateDate = now
            };
            db.Set<Organization>().Add(rootOrg);

            var defaultTenant = new Tenant
            {
                TenantCode = "default",
                TenantName = "默认租户",
                IsAvailable = "1",
                Status = "0",
                CreateDate = now,
                UpdateDate = now
            };
            db.Set<Tenant>().Add(defaultTenant);

            var tenantMenu = new Menu
            {
                MenuCode = IdGenerator.NewId(),
                MenuName = "租户管理",
                MenuHref = "/sys/tenant",
                MenuIcon = "team",
                Permission = "sys:tenant",
                IsShow = "1",
                ParentCode = sysMenu.MenuCode,
                TreeSort = 50,
                ModuleCode = "Sys",
                Status = "0",
                CreateDate = now,
                UpdateDate = now
            };
            db.Set<Menu>().Add(tenantMenu);

            await db.SaveChangesAsync();
        }
    }
}
