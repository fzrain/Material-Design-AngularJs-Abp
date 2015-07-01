using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Fzrain.Authorization.Roles;
using Fzrain.Users;

namespace Fzrain.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, Role, User>
    {
        public TenantManager(IRepository<Tenant> tenantRepository)
            : base(tenantRepository)
        {

        }
    }
}