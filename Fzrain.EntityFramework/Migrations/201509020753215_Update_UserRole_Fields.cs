namespace Fzrain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Update_UserRole_Fields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "Tenant_Id", "dbo.Tenant");
            DropForeignKey("dbo.Role", "Tenant_Id", "dbo.Tenant");
            DropIndex("dbo.Role", new[] { "Tenant_Id" });
            DropIndex("dbo.User", new[] { "Tenant_Id" });
            AddColumn("dbo.User", "MobilePhone", c => c.String(nullable: false, maxLength: 32));
            DropColumn("dbo.Role", "DisplayName");
            DropColumn("dbo.Role", "Tenant_Id");
            DropColumn("dbo.User", "Surname");
            DropColumn("dbo.User", "Tenant_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Tenant_Id", c => c.Int());
            AddColumn("dbo.User", "Surname", c => c.String(nullable: false, maxLength: 32));
            AddColumn("dbo.Role", "Tenant_Id", c => c.Int());
            AddColumn("dbo.Role", "DisplayName", c => c.String(nullable: false, maxLength: 64));
            DropColumn("dbo.User", "MobilePhone");
            CreateIndex("dbo.User", "Tenant_Id");
            CreateIndex("dbo.Role", "Tenant_Id");
            AddForeignKey("dbo.Role", "Tenant_Id", "dbo.Tenant", "Id");
            AddForeignKey("dbo.User", "Tenant_Id", "dbo.Tenant", "Id");
        }
    }
}
