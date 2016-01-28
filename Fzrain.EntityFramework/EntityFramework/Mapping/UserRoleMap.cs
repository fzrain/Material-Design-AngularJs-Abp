using System.Data.Entity.ModelConfiguration;
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
