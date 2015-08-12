using System.Data.Entity.ModelConfiguration;
using Fzrain.Auditing;

namespace Fzrain.EntityFramework.Mapping
{
   public  class AuditLogMap: EntityTypeConfiguration<AuditLog>
   {
       public AuditLogMap()
       {
           ToTable("AuditLog");
           Property(a => a.ServiceName).HasMaxLength(256);
           Property(a => a.MethodName).HasMaxLength(256);
           Property(a => a.Parameters).HasMaxLength(1024);
           Property(a => a.ClientIpAddress).HasMaxLength(64);
           Property(a => a.ClientName).HasMaxLength(128);
           Property(a => a.BrowserInfo).HasMaxLength(256);
           Property(a => a.Exception).HasMaxLength(2000);
       }
    }
}
