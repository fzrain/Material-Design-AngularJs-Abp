using System.Data.Entity.ModelConfiguration;
using Fzrain.Authorization.Permissions;

namespace Fzrain.EntityFramework.Mapping
{
    public class PermissionSettingMap : EntityTypeConfiguration<PermissionSetting>
    {
        public PermissionSettingMap()
        {
            ToTable("PermissionSetting");
            HasKey(p => p.Id);
            Property(p => p.Name).IsRequired().HasMaxLength(128);
        }
    }
}
