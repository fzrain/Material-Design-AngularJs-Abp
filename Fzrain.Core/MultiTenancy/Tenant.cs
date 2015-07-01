using Abp.MultiTenancy;
using Fzrain.Users;

namespace Fzrain.MultiTenancy
{
    public class Tenant : AbpTenant<Tenant, User>
    {

    }
}