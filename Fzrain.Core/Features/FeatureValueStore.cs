using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Fzrain.Authorization.Roles;
using Fzrain.MultiTenancy;
using Fzrain.Users;

namespace Fzrain.Features
{
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, Role, User>
    {
        public FeatureValueStore(ICacheManager cacheManager,IRepository<TenantFeatureSetting,long> tenantFeatureSettingRepository,IRepository<Tenant> tenantRepository, IRepository<EditionFeatureSetting, long> editionFeatureRepository,IFeatureManager featureManager, IUnitOfWorkManager unitOfWorkManager)
            : base(cacheManager, tenantFeatureSettingRepository, tenantRepository, editionFeatureRepository, featureManager, unitOfWorkManager)
        {
        }
    }
}