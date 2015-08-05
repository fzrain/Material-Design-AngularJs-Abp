using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Timing;
using Fzrain.Authorization.Permissions;
using Fzrain.Authorization.Roles;
using Fzrain.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace Fzrain.Authorization.Users
{
    /// <summary>
    /// Extends <see cref="UserManager{TUser,TKey}"/> of ASP.NET Identity Framework.
    /// </summary>
    public abstract class UserManager : UserManager<User, long>, ITransientDependency
      
    {
        private IUserPermissionStore UserPermissionStore
        {
            get
            {
                if (!(Store is IUserPermissionStore))
                {
                    throw new AbpException("Store is not IUserPermissionStore");
                }

                return Store as IUserPermissionStore;
            }
        }

        public ILocalizationManager LocalizationManager { get; set; }

        public IAbpSession AbpSession { get; set; }

        protected RoleManager RoleManager { get; private set; }

        protected ISettingManager SettingManager { get; private set; }

        protected UserStore AbpStore { get; private set; }

        private readonly IPermissionManager _permissionManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUserManagementConfig _userManagementConfig;
        private readonly IIocResolver _iocResolver;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        //TODO: Non-generic parameters may be converted to property-injection
        protected UserManager(
            UserStore userStore,
            RoleManager roleManager,
            IRepository<Tenant> tenantRepository,
            IMultiTenancyConfig multiTenancyConfig,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver)
            : base(userStore)
        {
            AbpStore = userStore;
            RoleManager = roleManager;
            SettingManager = settingManager;
            _tenantRepository = tenantRepository;
            _multiTenancyConfig = multiTenancyConfig;
            _permissionManager = permissionManager;
            _unitOfWorkManager = unitOfWorkManager;
            _userManagementConfig = userManagementConfig;
            _iocResolver = iocResolver;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        public override async Task<IdentityResult> CreateAsync(User user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            if (AbpSession.TenantId.HasValue)
            {
                user.TenantId = AbpSession.TenantId.Value;
            }

            return await base.CreateAsync(user);
        }

        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public virtual async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await IsGrantedAsync(
                await GetUserByIdAsync(userId),
                _permissionManager.GetPermission(permissionName)
                );
        }

        /// <summary>
        /// 检测一个用户是否有权限
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual async Task<bool> IsGrantedAsync(User user, Permission permission)
        {
            //Check for multi-tenancy side
            if (!permission.MultiTenancySides.HasFlag(AbpSession.MultiTenancySide))
            {
                return false;
            }

            //Check for user-specific value
            if (await UserPermissionStore.HasPermissionAsync(user, new PermissionGrantInfo(permission.Name, true)))
            {
                return true;
            }

            if (await UserPermissionStore.HasPermissionAsync(user, new PermissionGrantInfo(permission.Name, false)))
            {
                return false;
            }

            //Check for roles
            var roleNames = await GetRolesAsync(user.Id);
            if (!roleNames.Any())
            {
                return permission.IsGrantedByDefault;
            }

            foreach (var roleName in roleNames)
            {
                if (await RoleManager.HasPermissionAsync(roleName, permission.Name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets granted permissions for a user.
        /// </summary>
        /// <param name="user">Role</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(User user)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                if (await IsGrantedAsync(user, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }

        /// <summary>
        /// Sets all granted permissions of a user at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(User user, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedPermissionsAsync(user);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p)))
            {
                await ProhibitPermissionAsync(user, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p)))
            {
                await GrantPermissionAsync(user, permission);
            }
        }

        /// <summary>
        /// Prohibits all permissions for a user.
        /// </summary>
        /// <param name="user">User</param>
        public async Task ProhibitAllPermissionsAsync(User user)
        {
            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                await ProhibitPermissionAsync(user, permission);
            }
        }

        /// <summary>
        /// Resets all permission settings for a user.
        /// It removes all permission settings for the user.
        /// User will have permissions according to his roles.
        /// This method does not prohibit all permissions.
        /// For that, use <see cref="ProhibitAllPermissionsAsync"/>.
        /// </summary>
        /// <param name="user">User</param>
        public async Task ResetAllPermissionsAsync(User user)
        {
            await UserPermissionStore.RemoveAllPermissionSettingsAsync(user);
        }

        /// <summary>
        /// Grants a permission for a user if not already granted.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual async Task GrantPermissionAsync(User user, Permission permission)
        {
            await UserPermissionStore.RemovePermissionAsync(user, new PermissionGrantInfo(permission.Name, false));

            if (await IsGrantedAsync(user, permission))
            {
                return;
            }

            await UserPermissionStore.AddPermissionAsync(user, new PermissionGrantInfo(permission.Name, true));
        }

        /// <summary>
        /// Prohibits a permission for a user if it's granted.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual async Task ProhibitPermissionAsync(User user, Permission permission)
        {
            await UserPermissionStore.RemovePermissionAsync(user, new PermissionGrantInfo(permission.Name, true));

            if (!await IsGrantedAsync(user, permission))
            {
                return;
            }

            await UserPermissionStore.AddPermissionAsync(user, new PermissionGrantInfo(permission.Name, false));
        }

        public virtual async Task<User> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await AbpStore.FindByNameOrEmailAsync(userNameOrEmailAddress);
        }

        public virtual Task<List<User>> FindAllAsync(UserLoginInfo login)
        {
            return AbpStore.FindAllAsync(login);
        }

        [UnitOfWork]
        public virtual async Task<LoginResult> LoginAsync(UserLoginInfo login, string tenancyName = null)
        {
            if (login == null || login.LoginProvider.IsNullOrEmpty() || login.ProviderKey.IsNullOrEmpty())
            {
                throw new ArgumentException("login");                
            }

            //Get and check tenant
            Tenant tenant = null;
            if (!_multiTenancyConfig.IsEnabled)
            {
                tenant = await GetDefaultTenantAsync();
            }
            else if (!string.IsNullOrWhiteSpace(tenancyName))
            {
                tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                if (tenant == null)
                {
                    return new LoginResult(LoginResultType.InvalidTenancyName);
                }

                if (!tenant.IsActive)
                {
                    return new LoginResult(LoginResultType.TenantIsNotActive);
                }
            }

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var user = await AbpStore.FindAsync(tenant == null ? (int?)null : tenant.Id, login);
                if (user == null)
                {
                    return new LoginResult(LoginResultType.UnknownExternalLogin);
                }

                return await CreateLoginResultAsync(user);
            }
        }

        [UnitOfWork]
        public virtual async Task<LoginResult> LoginAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException("userNameOrEmailAddress");
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException("plainPassword");
            }

            //Get and check tenant
            Tenant tenant = null;
            if (!_multiTenancyConfig.IsEnabled)
            {
                tenant = await GetDefaultTenantAsync();
            }
            else if (!string.IsNullOrWhiteSpace(tenancyName))
            {
                tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                if (tenant == null)
                {
                    return new LoginResult(LoginResultType.InvalidTenancyName);
                }

                if (!tenant.IsActive)
                {
                    return new LoginResult(LoginResultType.TenantIsNotActive);
                }
            }

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var loggedInFromExternalSource = await TryLoginFromExternalAuthenticationSources(userNameOrEmailAddress, plainPassword, tenant);

                var user = await AbpStore.FindByNameOrEmailAsync(tenant == null ? (int?)null : tenant.Id, userNameOrEmailAddress);
                if (user == null)
                {
                    return new LoginResult(LoginResultType.InvalidUserNameOrEmailAddress);
                }

                if (!loggedInFromExternalSource)
                {
                    var verificationResult = new PasswordHasher().VerifyHashedPassword(user.Password, plainPassword);
                    if (verificationResult != PasswordVerificationResult.Success)
                    {
                        return new LoginResult(LoginResultType.InvalidPassword);
                    }
                }

                return await CreateLoginResultAsync(user);
            }
        }
        
        private async Task<LoginResult> CreateLoginResultAsync(User user)
        {
            if (!user.IsActive)
            {
                return new LoginResult(LoginResultType.UserIsNotActive);
            }

            if (await IsEmailConfirmationRequiredForLoginAsync(user.TenantId) && !user.IsEmailConfirmed)
            {
                return new LoginResult(LoginResultType.UserEmailIsNotConfirmed);
            }

            user.LastLoginTime = Clock.Now;

            await Store.UpdateAsync(user);

            await _unitOfWorkManager.Current.SaveChangesAsync();

            return new LoginResult(user, await CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie));
        }

        private async Task<bool> TryLoginFromExternalAuthenticationSources(string userNameOrEmailAddress, string plainPassword, Tenant tenant)
        {
            if (!_userManagementConfig.ExternalAuthenticationSources.Any())
            {
                return false;
            }

            foreach (var sourceType in _userManagementConfig.ExternalAuthenticationSources)
            {
                using (var source = _iocResolver.ResolveAsDisposable<IExternalAuthenticationSource>(sourceType))
                {
                    if (await source.Object.TryAuthenticateAsync(userNameOrEmailAddress, plainPassword, tenant))
                    {
                        var tenantId = tenant == null ? (int?) null : tenant.Id;

                        var user = await AbpStore.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
                        if (user == null)
                        {
                            user = await source.Object.CreateUserAsync(userNameOrEmailAddress, tenant);

                            user.Tenant = tenant;
                            user.AuthenticationSource = source.Object.Name;
                            user.Password = new PasswordHasher().HashPassword(Guid.NewGuid().ToString("N").Left(16)); //Setting a random password since it will not be used

                            user.Roles = new List<UserRole>();
                            foreach (var defaultRole in RoleManager.Roles.Where(r => r.TenantId == tenantId && r.IsDefault).ToList())
                            {
                                user.Roles.Add(new UserRole { RoleId = defaultRole.Id });
                            }

                            await Store.CreateAsync(user);
                        }
                        else
                        {
                            await source.Object.UpdateUserAsync(user, tenant);
                            
                            user.AuthenticationSource = source.Object.Name;

                            await Store.UpdateAsync(user);
                        }

                        await _unitOfWorkManager.Current.SaveChangesAsync();

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a user by given id.
        /// Throws exception if no user found with given id.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        /// <exception cref="AbpException">Throws exception if no user found with given id</exception>
        public virtual async Task<User> GetUserByIdAsync(long userId)
        {
            var user = await FindByIdAsync(userId);
            if (user == null)
            {
                throw new AbpException("There is no user with id: " + userId);
            }

            return user;
        }

        public async override Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType)
        {
            var identity = await base.CreateIdentityAsync(user, authenticationType);
            if (user.TenantId.HasValue)
            {
                identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString(CultureInfo.InvariantCulture)));
            }

            return identity;
        }

        public async override Task<IdentityResult> UpdateAsync(User user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            var oldUserName = (await GetUserByIdAsync(user.Id)).UserName;
            if (oldUserName == User.AdminUserName && user.UserName != User.AdminUserName)
            {
                return IdentityResult.Failed(string.Format(L("CanNotRenameAdminUser"), User.AdminUserName));
            }

            return await base.UpdateAsync(user);
        }

        public async override Task<IdentityResult> DeleteAsync(User user)
        {
            if (user.UserName == User.AdminUserName)
            {
                return IdentityResult.Failed(string.Format(L("CanNotDeleteAdminUser"), User.AdminUserName));
            }

            return await base.DeleteAsync(user);
        }

        public virtual async Task<IdentityResult> ChangePasswordAsync(User user, string newPassword)
        {
            var result = await PasswordValidator.ValidateAsync(newPassword);
            if (!result.Succeeded)
            {
                return result;
            }

            await AbpStore.SetPasswordHashAsync(user, PasswordHasher.HashPassword(newPassword));
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressAsync(long? expectedUserId, string userName, string emailAddress)
        {
            var user = (await FindByNameAsync(userName));
            if (user != null && user.Id != expectedUserId)
            {
                return IdentityResult.Failed(string.Format(L("Identity.DuplicateName"), userName));
            }

            user = (await FindByEmailAsync(emailAddress));
            if (user != null && user.Id != expectedUserId)
            {
                return IdentityResult.Failed(string.Format(L("Identity.DuplicateEmail"), emailAddress));
            }

            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> SetRoles(User user, string[] roleNames)
        {
            //Remove from removed roles
            foreach (var userRole in user.Roles.ToList())
            {
                var role = await RoleManager.FindByIdAsync(userRole.RoleId);
                if (roleNames.All(roleName => role.Name != roleName))
                {
                    var result = await RemoveFromRoleAsync(user.Id, role.Name);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }

            //Add to added roles
            foreach (var roleName in roleNames)
            {
                var role = await RoleManager.GetRoleByNameAsync(roleName);
                if (user.Roles.All(ur => ur.RoleId != role.Id))
                {
                    var result = await AddToRoleAsync(user.Id, roleName);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }

            return IdentityResult.Success;
        }

        private async Task<bool> IsEmailConfirmationRequiredForLoginAsync(int? tenantId)
        {
            if (tenantId.HasValue)
            {
                return await SettingManager.GetSettingValueForTenantAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, tenantId.Value);
            }

            return await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
        }

        private async Task<Tenant> GetDefaultTenantAsync()
        {
            var tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == Tenant.DefaultTenantName);
            if (tenant == null)
            {
                throw new AbpException("There should be a 'Default' tenant if multi-tenancy is disabled!");
            }

            return tenant;
        }

        private string L(string name)
        {
            return LocalizationManager.GetString(FzrainConsts.LocalizationSourceName, name);
        }

        public class LoginResult
        {
            public LoginResultType Result { get; private set; }

            public User User { get; private set; }

            public ClaimsIdentity Identity { get; private set; }

            public LoginResult(LoginResultType result)
            {
                Result = result;
            }

            public LoginResult(User user, ClaimsIdentity identity)
                : this(LoginResultType.Success)
            {
                User = user;
                Identity = identity;
            }
        }
    }
}