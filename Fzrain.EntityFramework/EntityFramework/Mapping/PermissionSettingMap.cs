using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fzrain.Authorization.Permissions;

namespace Fzrain.EntityFramework.Mapping
{
    public class PermissionSettingMap : EntityTypeConfiguration<PermissionSetting>
    {
        public PermissionSettingMap()
        {
            HasKey(p => p.Id);
        }
    }
}
