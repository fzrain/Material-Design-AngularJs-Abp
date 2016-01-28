namespace Fzrain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Update_Table_Struct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User_R_Role", "User_Id", "dbo.User");
            DropForeignKey("dbo.User_R_Role", "Role_Id", "dbo.Role");
            DropIndex("dbo.User_R_Role", new[] { "User_Id" });
            DropIndex("dbo.User_R_Role", new[] { "Role_Id" });
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            DropTable("dbo.User_R_Role");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.User_R_Role",
                c => new
                    {
                        User_Id = c.Long(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Role_Id });
            
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropTable("dbo.UserRole");
            CreateIndex("dbo.User_R_Role", "Role_Id");
            CreateIndex("dbo.User_R_Role", "User_Id");
            AddForeignKey("dbo.User_R_Role", "Role_Id", "dbo.Role", "Id", cascadeDelete: true);
            AddForeignKey("dbo.User_R_Role", "User_Id", "dbo.User", "Id", cascadeDelete: true);
        }
    }
}
