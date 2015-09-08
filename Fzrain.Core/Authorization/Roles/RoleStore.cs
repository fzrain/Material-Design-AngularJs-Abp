using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Fzrain.Authorization.Permissions;
using Microsoft.AspNet.Identity;

namespace Fzrain.Authorization.Roles
{
    /// <summary>
    /// Implements 'Role Store' of ASP.NET Identity Framework.
    /// </summary>
    public class RoleStore :
        IQueryableRoleStore<Role, int>,
        IRolePermissionStore,
        ITransientDependency
    
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<PermissionSetting, long> _permissionSettingRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        public  RoleStore(
            IRepository<Role> roleRepository, 
            IRepository<PermissionSetting, long> permissionSettingRepository)
        {
            _roleRepository = roleRepository;
            _permissionSettingRepository = permissionSettingRepository;
        }

        public virtual IQueryable<Role> Roles => _roleRepository.GetAll();

        public virtual async Task CreateAsync(Role role)
        {
            await _roleRepository.InsertAsync(role);
        }

        public virtual async Task UpdateAsync(Role role)
        {
            await _roleRepository.UpdateAsync(role);
        }

        public virtual async Task DeleteAsync(Role role)
        {         
            await _roleRepository.DeleteAsync(role);
        }

        public virtual async Task<Role> FindByIdAsync(int roleId)
        {
            return await _roleRepository.FirstOrDefaultAsync(roleId);
        }

        public virtual async Task<Role> FindByNameAsync(string roleName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role => role.Name == roleName
                );
        }
        /// <inheritdoc/>
        public virtual async Task AddPermissionAsync(Role role, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(role, permissionGrant))
            {
                return;
            }

            await _permissionSettingRepository.InsertAsync(
                new PermissionSetting
                {
                    RoleId = role.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }

        /// <inheritdoc/>
        public virtual async Task RemovePermissionAsync(Role role, PermissionGrantInfo permissionGrant)
        {
            await _permissionSettingRepository.DeleteAsync(
                permissionSetting => permissionSetting.RoleId == role.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
                );
        }

        /// <inheritdoc/>
        public virtual async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(Role role)
        {
            return (await _permissionSettingRepository.GetAllListAsync(p => p.RoleId == role.Id))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }

        /// <inheritdoc/>
        public virtual async Task<bool> HasPermissionAsync(Role role, PermissionGrantInfo permissionGrant)
        {
            return await _permissionSettingRepository.FirstOrDefaultAsync(
                p => p.RoleId == role.Id &&
                     p.Name == permissionGrant.Name &&
                     p.IsGranted == permissionGrant.IsGranted
                ) != null;
        }

        /// <inheritdoc/>
        public virtual async Task RemoveAllPermissionSettingsAsync(Role role)
        {
            await _permissionSettingRepository.DeleteAsync(s => s.RoleId == role.Id);
        }

        public virtual void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
