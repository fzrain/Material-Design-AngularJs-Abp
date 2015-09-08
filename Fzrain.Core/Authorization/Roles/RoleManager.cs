using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Fzrain.Authorization.Permissions;
using Microsoft.AspNet.Identity;

namespace Fzrain.Authorization.Roles
{
    /// <summary>
    /// Extends <see cref="RoleManager{TRole,TKey}"/> of ASP.NET Identity Framework.
    /// Applications should derive this class with appropriate generic arguments.
    /// </summary>
    public  class RoleManager: RoleManager<Role, int>, ITransientDependency
      
    {
        public ILocalizationManager LocalizationManager { get; set; }

        public IAbpSession AbpSession { get; set; }
        
        public IRoleManagementConfig RoleManagementConfig { get; }

        private IRolePermissionStore RolePermissionStore
        {
            get
            {
                if (!(Store is IRolePermissionStore))
                {
                    throw new AbpException("Store is not IRolePermissionStore");
                }

                return (IRolePermissionStore) Store;
            }
        }

        protected RoleStore AbpStore { get; }

        private readonly IPermissionManager _permissionManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public  RoleManager(
            RoleStore store, 
            IPermissionManager permissionManager, 
            IRoleManagementConfig roleManagementConfig)
            : base(store)
        {
            RoleManagementConfig = roleManagementConfig;
            _permissionManager = permissionManager;
            AbpStore = store;
            AbpSession = NullAbpSession.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="roleName">The role's name to check it's permission</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>True, if the role has the permission</returns>
        public virtual async Task<bool> HasPermissionAsync(string roleName, string permissionName)
        {
            return await HasPermissionAsync(await GetRoleByNameAsync(roleName), _permissionManager.GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="roleId">The role's id to check it's permission</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>True, if the role has the permission</returns>
        public virtual async Task<bool> HasPermissionAsync(int roleId, string permissionName)
        {
            return await HasPermissionAsync(await GetRoleByIdAsync(roleId), _permissionManager.GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="role">The rolepermission</param>
        /// <param name="permission">The permission</param>
        /// <returns>True, if the role has the permission</returns>
        public async Task<bool> HasPermissionAsync(Role role, Permission permission)
        {
            return permission.IsGrantedByDefault
                ? !(await RolePermissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permission.Name, false)))
                : (await RolePermissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permission.Name, true)));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(int roleId)
        {
            return await GetGrantedPermissionsAsync(await GetRoleByIdAsync(roleId));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(string roleName)
        {
            return await GetGrantedPermissionsAsync(await GetRoleByNameAsync(roleName));
        }

        /// <summary>
        /// Gets granted permissions for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(Role role)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                if (await HasPermissionAsync(role, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }

        /// <summary>
        /// Sets all granted permissions of a role at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(int roleId, IEnumerable<Permission> permissions)
        {
            await SetGrantedPermissionsAsync(await GetRoleByIdAsync(roleId), permissions);
        }

        /// <summary>
        /// Sets all granted permissions of a role at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedPermissionsAsync(role);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p)))
            {
                await ProhibitPermissionAsync(role, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p)))
            {
                await GrantPermissionAsync(role, permission);
            }
        }

        /// <summary>
        /// Grants a permission for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permission">Permission</param>
        public async Task GrantPermissionAsync(Role role, Permission permission)
        {
            if (await HasPermissionAsync(role, permission))
            {
                return;
            }

            if (permission.IsGrantedByDefault)
            {
                await RolePermissionStore.RemovePermissionAsync(role, new PermissionGrantInfo(permission.Name, false));
            }
            else
            {
                await RolePermissionStore.AddPermissionAsync(role, new PermissionGrantInfo(permission.Name, true));
            }
        }

        /// <summary>
        /// Prohibits a permission for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permission">Permission</param>
        public async Task ProhibitPermissionAsync(Role role, Permission permission)
        {
            if (!await HasPermissionAsync(role, permission))
            {
                return;
            }

            if (permission.IsGrantedByDefault)
            {
                await RolePermissionStore.AddPermissionAsync(role, new PermissionGrantInfo(permission.Name, false));
            }
            else
            {
                await RolePermissionStore.RemovePermissionAsync(role, new PermissionGrantInfo(permission.Name, true));
            }
        }

        /// <summary>
        /// Prohibits all permissions for a role.
        /// </summary>
        /// <param name="role">Role</param>
        public async Task ProhibitAllPermissionsAsync(Role role)
        {
            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                await ProhibitPermissionAsync(role, permission);
            }
        }

        /// <summary>
        /// Resets all permission settings for a role.
        /// It removes all permission settings for the role.
        /// Role will have permissions those have <see cref="Permission.IsGrantedByDefault"/> set to true.
        /// </summary>
        /// <param name="role">Role</param>
        public async Task ResetAllPermissionsAsync(Role role)
        {
            await RolePermissionStore.RemoveAllPermissionSettingsAsync(role);
        }

        /// <summary>
        /// Creates a role.
        /// </summary>
        /// <param name="role">Role</param>
        public override async Task<IdentityResult> CreateAsync(Role role)
        {
            var result = await CheckDuplicateRoleNameAsync(role.Id, role.Name);
            if (!result.Succeeded)
            {
                return result;
            }

            if (AbpSession.TenantId.HasValue)
            {
                role.TenantId = AbpSession.TenantId.Value;
            }
            
            return await base.CreateAsync(role);
        }

        public override async Task<IdentityResult> UpdateAsync(Role role)
        {
            var result = await CheckDuplicateRoleNameAsync(role.Id, role.Name);
            if (!result.Succeeded)
            {
                return result;
            }

            return await base.UpdateAsync(role);
        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="role">Role</param>
        public async override Task<IdentityResult> DeleteAsync(Role role)
        {
            if (role.IsStatic)
            {
                return IdentityResult.Failed(string.Format(L("CanNotDeleteStaticRole"), role.Name));
            }

            return await base.DeleteAsync(role);
        }

        /// <summary>
        /// Gets a role by given id.
        /// Throws exception if no role with given id.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>Role</returns>
        /// <exception cref="AbpException">Throws exception if no role with given id</exception>
        public virtual async Task<Role> GetRoleByIdAsync(int roleId)
        {
            var role = await FindByIdAsync(roleId);
            if (role == null)
            {
                throw new AbpException("There is no role with id: " + roleId);
            }

            return role;
        }

        /// <summary>
        /// Gets a role by given name.
        /// Throws exception if no role with given roleName.
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>Role</returns>
        /// <exception cref="AbpException">Throws exception if no role with given roleName</exception>
        public virtual async Task<Role> GetRoleByNameAsync(string roleName)
        {
            var role = await FindByNameAsync(roleName);
            if (role == null)
            {
                throw new AbpException("There is no role with name: " + roleName);
            }

            return role;
        }

        public async Task GrantAllPermissionsAsync(Role role)
        {
            var permissions = _permissionManager.GetAllPermissions(role.GetMultiTenancySide());
            await SetGrantedPermissionsAsync(role, permissions);
        }
        
        public virtual async Task<IdentityResult> CreateStaticRoles(int tenantId)
        {
            var staticRoleDefinitions = RoleManagementConfig.StaticRoles.Where(sr => sr.Side == MultiTenancySides.Tenant);

            foreach (var staticRoleDefinition in staticRoleDefinitions)
            {
                var role = new Role
                {
                    TenantId = tenantId,
                    Name = staticRoleDefinition.RoleName,                 
                    IsStatic = true
                };

                var identityResult = await CreateAsync(role);
                if (!identityResult.Succeeded)
                {
                    return identityResult;
                }
            }

            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> CheckDuplicateRoleNameAsync(int? expectedRoleId, string name)
        {
            var role = await FindByNameAsync(name);
            if (role != null && role.Id != expectedRoleId)
            {
                return IdentityResult.Failed(string.Format(L("RoleNameIsAlreadyTaken"), name));
            }       
            return IdentityResult.Success;
        }

       

        private string L(string name)
        {
            return LocalizationManager.GetString(FzrainConsts.LocalizationSourceName, name);
        }
    }
}