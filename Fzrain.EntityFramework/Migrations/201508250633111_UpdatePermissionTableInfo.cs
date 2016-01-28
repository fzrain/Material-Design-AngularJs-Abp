namespace Fzrain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePermissionTableInfo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Permissions", "ParentPermission_Id", "dbo.Permissions");
            DropIndex("dbo.Permissions", new[] { "ParentPermission_Id" });
            AddColumn("dbo.Permissions", "ParentName", c => c.String());
            DropColumn("dbo.Permissions", "ParentPermission_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Permissions", "ParentPermission_Id", c => c.Int());
            DropColumn("dbo.Permissions", "ParentName");
            CreateIndex("dbo.Permissions", "ParentPermission_Id");
            AddForeignKey("dbo.Permissions", "ParentPermission_Id", "dbo.Permissions", "Id");
        }
    }
}
