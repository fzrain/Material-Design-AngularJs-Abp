using System.Data.Entity.ModelConfiguration;
using Fzrain.Authorization.Users;

namespace Fzrain.EntityFramework.Mapping
{
   public  class UserLoginMap: EntityTypeConfiguration<UserLogin>
   {
       public UserLoginMap()
       {
           ToTable("UserLogin");
           Property(l => l.LoginProvider).IsRequired().HasMaxLength(128);
           Property(l => l.ProviderKey).IsRequired().HasMaxLength(256);
       }
    }
}
