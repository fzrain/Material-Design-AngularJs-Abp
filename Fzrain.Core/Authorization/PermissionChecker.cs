using Abp.Authorization;
using Fzrain.Authorization.Roles;
using Fzrain.MultiTenancy;
using Fzrain.Users;

namespace Fzrain.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
