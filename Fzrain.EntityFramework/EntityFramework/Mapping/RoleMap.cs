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
           Property(r => r.DisplayName).IsRequired().HasMaxLength(64);
           HasOptional(r => r.Tenant).WithOptionalDependent();
           HasMany(r => r.Permissions).WithOptional().HasForeignKey(p => p.RoleId);
       }
    }
}
