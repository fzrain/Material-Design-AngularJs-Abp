using System.Linq;
using Fzrain.Authorization.Roles;
using Fzrain.Authorization.Users;
using Fzrain.EntityFramework;
using Fzrain.MultiTenancy;

namespace Fzrain.Migrations.SeedData
{
    public class DefaultTenantRoleAndUserBuilder
    {
        private readonly FzrainDbContext _context;

        public DefaultTenantRoleAndUserBuilder(FzrainDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Admin role for tenancy owner

            //var adminRoleForTenancyOwner = _context.Set<Role>().FirstOrDefault(r => r.TenantId == null && r.Name == "系统管理员");
            //if (adminRoleForTenancyOwner == null)
            //{
            //    adminRoleForTenancyOwner = _context.Set<Role>().Add(new Role { Name = "系统管理员" });
            //    _context.SaveChanges();
            //}

            //Admin user for tenancy owner

            //var adminUserForTenancyOwner = _context.Set<User>().FirstOrDefault(u => u.TenantId == null && u.UserName == "admin");
            //if (adminUserForTenancyOwner == null)
            //{
            //    adminUserForTenancyOwner = _context.Set<User>().Add(
            //        new User
            //        {
            //            TenantId = null,
            //            UserName = "admin",
            //            Name = "系统管理员",                      
            //            EmailAddress = "admin@aspnetboilerplate.com",
            //            IsEmailConfirmed = true,
            //            Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==", //123qwe
                      
            //        });
            //    adminUserForTenancyOwner.Roles.Add(adminRoleForTenancyOwner);
            //    _context.SaveChanges();

              
            //}

            //Default tenant

            var defaultTenant = _context.Set<Tenant>().FirstOrDefault(t => t.TenancyName == "Default");
            if (defaultTenant == null)
            {
                defaultTenant = _context.Set<Tenant>().Add(new Tenant { TenancyName = "Default", Name = "Default" });
                _context.SaveChanges();
            }

            //Admin role for 'Default' tenant

            var adminRoleForDefaultTenant = _context.Set<Role>().FirstOrDefault(r => r.TenantId == defaultTenant.Id && r.Name == "系统管理员");
            if (adminRoleForDefaultTenant == null)
            {
                adminRoleForDefaultTenant = _context.Set<Role>().Add(new Role { TenantId = defaultTenant.Id, Name = "系统管理员" });
                _context.SaveChanges();
            }

            //Admin for 'Default' tenant

            var adminUserForDefaultTenant = _context.Set<User>().FirstOrDefault(u => u.TenantId == defaultTenant.Id && u.UserName == "admin");
            if (adminUserForDefaultTenant == null)
            {
                adminUserForDefaultTenant = _context.Set<User>().Add(
                    new User
                    {
                        TenantId = defaultTenant.Id,
                        UserName = "admin",
                        Name = "系统管理员",                   
                        EmailAddress = "fzrain@hotmail.com",
                        IsEmailConfirmed = true,
                        Password = "AAe3pfZkLFrQyn3CsM344UTUP7qtr97rDrXn/gfEoPua/bSsdFIhYX7jAMtWMHSaRw==" //111111
                    });
                adminUserForDefaultTenant.Roles.Add(adminRoleForDefaultTenant);               
                _context.SaveChanges();
            }
        }
    }
}