using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Localization;
using Microsoft.AspNet.Identity;

namespace Fzrain.MultiTenancy
{
    public class TenantManager : ITransientDependency
    {
        public ILocalizationManager LocalizationManager { get; set; }

        private readonly IRepository<Tenant> _tenantRepository;

        protected TenantManager(IRepository<Tenant> tenantRepository)
        {
            _tenantRepository = tenantRepository;

            LocalizationManager = NullLocalizationManager.Instance;
        }

        public virtual IQueryable<Tenant> Tenants { get { return _tenantRepository.GetAll(); } }

        public virtual async Task<IdentityResult> CreateAsync(Tenant tenant)
        {
            if (await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenant.TenancyName) != null)
            {
                return IdentityResult.Failed(string.Format(L("TenancyNameIsAlreadyTaken"), tenant.TenancyName));
            }

            var validationResult = await ValidateTenantAsync(tenant);
            if (!validationResult.Succeeded)
            {
                return validationResult;
            }

            await _tenantRepository.InsertAsync(tenant);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(Tenant tenant)
        {
            if (await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenant.TenancyName && t.Id != tenant.Id) != null)
            {
                return IdentityResult.Failed(string.Format(L("TenancyNameIsAlreadyTaken"), tenant.TenancyName));
            }

            await _tenantRepository.UpdateAsync(tenant);
            return IdentityResult.Success;
        }

        public virtual async Task<Tenant> FindByIdAsync(int id)
        {
            return await _tenantRepository.FirstOrDefaultAsync(id);
        }

        public virtual async Task<Tenant> GetByIdAsync(int id)
        {
            var tenant = await FindByIdAsync(id);
            if (tenant == null)
            {
                throw new AbpException("There is no tenant with id: " + id);
            }

            return tenant;
        }

        public virtual Task<Tenant> FindByTenancyNameAsync(string tenancyName)
        {
            return _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
        }

        public virtual async Task<IdentityResult> DeleteAsync(Tenant tenant)
        {
            await _tenantRepository.DeleteAsync(tenant);
            return IdentityResult.Success;
        }

        protected virtual async Task<IdentityResult> ValidateTenantAsync(Tenant tenant)
        {
            var nameValidationResult = await ValidateTenancyNameAsync(tenant.TenancyName);
            if (!nameValidationResult.Succeeded)
            {
                return nameValidationResult;
            }

            return IdentityResult.Success;
        }

        protected virtual async Task<IdentityResult> ValidateTenancyNameAsync(string tenancyName)
        {
            if (!Regex.IsMatch(tenancyName, Tenant.TenancyNameRegex))
            {
                return IdentityResult.Failed(L("InvalidTenancyName"));
            }

            return IdentityResult.Success;
        }

        private string L(string name)
        {
            return LocalizationManager.GetString(FzrainConsts.LocalizationSourceName, name);
        }
    }
}