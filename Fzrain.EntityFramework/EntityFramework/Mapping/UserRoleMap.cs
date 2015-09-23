using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fzrain.Authorization.Users;

namespace Fzrain.EntityFramework.Mapping
{
    public  class UserRoleMap: EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            ToTable("UserRole");
        }
    }
}
