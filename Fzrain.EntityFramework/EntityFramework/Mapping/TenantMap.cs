using System.Data.Entity.ModelConfiguration;
using Fzrain.MultiTenancy;

namespace Fzrain.EntityFramework.Mapping
{
    public class TenantMap: EntityTypeConfiguration<Tenant>
    {
        public TenantMap()
        {
            ToTable("Tenant");
            HasKey(t => t.Id);
            Property(t => t.TenancyName).IsRequired().HasMaxLength(64);
            Property(t => t.Name).IsRequired().HasMaxLength(128);
            HasMany(t => t.Settings).WithOptional().HasForeignKey(c => c.TenantId);
        }
    }
}
