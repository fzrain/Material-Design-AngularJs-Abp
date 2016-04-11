namespace Fzrain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVersion084 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Tenant", newName: "Tenants");
            DropIndex("dbo.UserLoginAttempts", new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });
            AlterColumn("dbo.UserLoginAttempts", "UserNameOrEmailAddress", c => c.String(maxLength: 255));
            CreateIndex("dbo.UserLoginAttempts", new[] { "UserId", "TenantId" });
            CreateIndex("dbo.UserLoginAttempts", new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserLoginAttempts", new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });
            DropIndex("dbo.UserLoginAttempts", new[] { "UserId", "TenantId" });
            AlterColumn("dbo.UserLoginAttempts", "UserNameOrEmailAddress", c => c.String(maxLength: 256));
            CreateIndex("dbo.UserLoginAttempts", new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });
            RenameTable(name: "dbo.Tenants", newName: "Tenant");
        }
    }
}
