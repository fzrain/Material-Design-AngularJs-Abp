using System.Data.Entity.ModelConfiguration;
using Fzrain.Authorization.Roles;

namespace Fzrain.EntityFramework.Mapping
{
   public  class RoleMap: EntityTypeConfiguration<Role>
   {
       public RoleMap()
       {
           ToTable("Role");
           Property(r => r.Name).IsRequired().HasMaxLength(32);
          // HasOptional(r => r.Tenant).WithOptionalDependent().Map(c=>c.MapKey("TenantId"));
           HasMany(r => r.Permissions).WithOptional().HasForeignKey(p => p.RoleId);
       }
    }
}
