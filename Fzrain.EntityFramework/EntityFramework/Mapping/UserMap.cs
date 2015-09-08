using System.Data.Entity.ModelConfiguration;
using Fzrain.Authorization.Users;

namespace Fzrain.EntityFramework.Mapping
{
    public  class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("User");
            HasKey(u => u.Id);
            Property(u => u.UserName).IsRequired().HasMaxLength(32);
            Property(u => u.Password).IsRequired().HasMaxLength(128);
            Property(u => u.AuthenticationSource).HasMaxLength(64);
            Property(u => u.Name).IsRequired().HasMaxLength(32);
            Property(u => u.MobilePhone).IsRequired().HasMaxLength(32);
            Property(u => u.EmailAddress).IsRequired().HasMaxLength(256);
            Property(u => u.EmailConfirmationCode).HasMaxLength(128);
            Property(u => u.PasswordResetCode).HasMaxLength(128);
           // HasOptional(u => u.Tenant).WithOptionalDependent().Map(c=>c.MapKey("TenantId"));
            HasMany(u => u.Roles).WithMany(r => r.Users).Map(c => c.ToTable("User_R_Role"));
            HasMany(u => u.Logins).WithRequired().HasForeignKey(l => l.UserId);
            HasMany(u => u.Permissions).WithOptional().HasForeignKey(p => p.UserId);
            HasMany(u => u.Settings).WithOptional().HasForeignKey(s => s.UserId);
        }
    }
}
