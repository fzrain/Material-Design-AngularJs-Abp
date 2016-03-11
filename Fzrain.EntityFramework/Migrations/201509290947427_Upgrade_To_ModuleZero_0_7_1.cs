using System.Data.Entity.Migrations;

namespace Fzrain.Migrations
{
    public partial class Upgrade_To_ModuleZero_0_7_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpAuditLogs", "ImpersonatorUserId", c => c.Long());
            AddColumn("dbo.AbpAuditLogs", "ImpersonatorTenantId", c => c.Int());
            AddColumn("dbo.AbpAuditLogs", "CustomData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpAuditLogs", "CustomData");
            DropColumn("dbo.AbpAuditLogs", "ImpersonatorTenantId");
            DropColumn("dbo.AbpAuditLogs", "ImpersonatorUserId");
        }
    }
}
