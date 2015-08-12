using System.Data.Entity.ModelConfiguration;
using Fzrain.Authorization.Permissions;

namespace Fzrain.EntityFramework.Mapping
{
    public class PermissionInfoMap : EntityTypeConfiguration<PermissionInfo>
    {
        public PermissionInfoMap()
        {
            ToTable("Permissions");
            HasKey(p => p.Id);
            Property(p => p.Description).HasMaxLength(200);
            HasOptional(m => m.ParentPermission).WithMany(d => d.ChildrenPermissions);   
        }
    }
}
