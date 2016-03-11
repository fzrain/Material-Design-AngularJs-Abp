namespace Fzrain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Db_Structs : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AbpAuditLogs", newName: "AuditLogs");
            RenameTable(name: "dbo.AbpBackgroundJobs", newName: "BackgroundJobs");
            RenameTable(name: "dbo.AbpFeatures", newName: "Features");
            RenameTable(name: "dbo.AbpEditions", newName: "Editions");
            RenameTable(name: "dbo.AbpLanguages", newName: "Languages");
            RenameTable(name: "dbo.AbpLanguageTexts", newName: "LanguageTexts");
            RenameTable(name: "dbo.AbpNotifications", newName: "Notifications");
            RenameTable(name: "dbo.AbpNotificationSubscriptions", newName: "NotificationSubscriptions");
            RenameTable(name: "dbo.AbpOrganizationUnits", newName: "OrganizationUnits");
            RenameTable(name: "dbo.AbpPermissions", newName: "Permissions");
            RenameTable(name: "dbo.AbpRoles", newName: "Roles");
            RenameTable(name: "dbo.AbpUsers", newName: "Users");
            RenameTable(name: "dbo.AbpUserLogins", newName: "UserLogins");
            RenameTable(name: "dbo.AbpUserRoles", newName: "UserRoles");
            RenameTable(name: "dbo.AbpSettings", newName: "Settings");
            RenameTable(name: "dbo.AbpTenants", newName: "Tenant");
            RenameTable(name: "dbo.AbpUserNotifications", newName: "UserNotifications");
            RenameTable(name: "dbo.AbpUserOrganizationUnits", newName: "UserOrganizationUnits");
            CreateTable(
                "dbo.PermissionInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        DisplayName = c.String(),
                        IsGrantedByDefault = c.Boolean(nullable: false),
                        Name = c.String(),
                        ParentName = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.UserLoginAttempts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        TenancyName = c.String(maxLength: 64),
                        UserId = c.Long(),
                        UserNameOrEmailAddress = c.String(maxLength: 256),
                        ClientIpAddress = c.String(maxLength: 64),
                        ClientName = c.String(maxLength: 128),
                        BrowserInfo = c.String(maxLength: 256),
                        Result = c.Byte(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.TenancyName, t.UserNameOrEmailAddress, t.Result });
            
            AddColumn("dbo.Users", "MobilePhone", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PermissionInfoes", "CreatorUserId", "dbo.Users");
            DropIndex("dbo.UserLoginAttempts", new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });
            DropIndex("dbo.PermissionInfoes", new[] { "CreatorUserId" });
            DropColumn("dbo.Users", "MobilePhone");
            DropTable("dbo.UserLoginAttempts");
            DropTable("dbo.PermissionInfoes");
            RenameTable(name: "dbo.UserOrganizationUnits", newName: "AbpUserOrganizationUnits");
            RenameTable(name: "dbo.UserNotifications", newName: "AbpUserNotifications");
            RenameTable(name: "dbo.Tenant", newName: "AbpTenants");
            RenameTable(name: "dbo.Settings", newName: "AbpSettings");
            RenameTable(name: "dbo.UserRoles", newName: "AbpUserRoles");
            RenameTable(name: "dbo.UserLogins", newName: "AbpUserLogins");
            RenameTable(name: "dbo.Users", newName: "AbpUsers");
            RenameTable(name: "dbo.Roles", newName: "AbpRoles");
            RenameTable(name: "dbo.Permissions", newName: "AbpPermissions");
            RenameTable(name: "dbo.OrganizationUnits", newName: "AbpOrganizationUnits");
            RenameTable(name: "dbo.NotificationSubscriptions", newName: "AbpNotificationSubscriptions");
            RenameTable(name: "dbo.Notifications", newName: "AbpNotifications");
            RenameTable(name: "dbo.LanguageTexts", newName: "AbpLanguageTexts");
            RenameTable(name: "dbo.Languages", newName: "AbpLanguages");
            RenameTable(name: "dbo.Editions", newName: "AbpEditions");
            RenameTable(name: "dbo.Features", newName: "AbpFeatures");
            RenameTable(name: "dbo.BackgroundJobs", newName: "AbpBackgroundJobs");
            RenameTable(name: "dbo.AuditLogs", newName: "AbpAuditLogs");
        }
    }
}
