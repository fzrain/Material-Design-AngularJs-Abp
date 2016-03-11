using Abp.Application.Features;
using Fzrain.Authorization.Roles;
using Fzrain.MultiTenancy;
using Fzrain.Users;

namespace Fzrain.Features
{
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, Role, User>
    {
        public FeatureValueStore(TenantManager tenantManager)
            : base(tenantManager)
        {
        }
    }
}