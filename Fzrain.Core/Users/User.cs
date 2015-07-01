using Abp.Authorization.Users;
using Fzrain.MultiTenancy;

namespace Fzrain.Users
{
    public class User : AbpUser<Tenant, User>
    {

    }
}