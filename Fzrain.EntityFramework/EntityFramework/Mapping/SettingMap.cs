using System.Data.Entity.ModelConfiguration;
using Fzrain.Configuration;

namespace Fzrain.EntityFramework.Mapping
{
    public  class SettingMap: EntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            ToTable("Setting");
            Property(s => s.Name).IsRequired().HasMaxLength(256);
            Property(s => s.Value).HasMaxLength(2000);
        }
    }
}
