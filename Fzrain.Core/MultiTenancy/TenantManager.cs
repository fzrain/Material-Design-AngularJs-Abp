using Abp.Application.Editions;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Fzrain.Authorization.Roles;
using Fzrain.Editions;
using Fzrain.Users;

namespace Fzrain.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, Role, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
           EditionManager editionManager) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager
            )
        {
        }
    }
}