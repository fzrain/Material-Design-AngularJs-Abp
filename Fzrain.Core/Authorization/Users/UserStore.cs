using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Fzrain.Authorization.Permissions;
using Fzrain.Authorization.Roles;
using Fzrain.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace Fzrain.Authorization.Users
{
    /// <summary>
    /// Implements 'User Store' of ASP.NET Identity Framework.
    /// </summary>
    public abstract class UserStore :
        IUserPasswordStore<User, long>,
        IUserEmailStore<User, long>,
        IUserLoginStore<User, long>,
        IUserRoleStore<User, long>,
        IQueryableUserStore<User, long>,
        IUserPermissionStore,
        ITransientDependency
       
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserLogin, long> _userLoginRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionSettingRepository;
        private readonly IAbpSession _session;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected UserStore(
            IRepository<User, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
            IAbpSession session,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _session = session;
            _unitOfWorkManager = unitOfWorkManager;
            _userPermissionSettingRepository = userPermissionSettingRepository;
        }

        public virtual async Task CreateAsync(User user)
        {
            await _userRepository.InsertAsync(user);
        }

        public virtual async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public virtual async Task DeleteAsync(User user)
        {
            await _userRepository.DeleteAsync(user.Id);
        }

        public virtual async Task<User> FindByIdAsync(long userId)
        {
            return await _userRepository.FirstOrDefaultAsync(userId);
        }

        public virtual async Task<User> FindByNameAsync(string userName)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.UserName == userName
                );
        }

        public virtual async Task<User> FindByEmailAsync(string email)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.EmailAddress == email
                );
        }

        /// <summary>
        /// Tries to find a user with user name or email address.
        /// </summary>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <returns>User or null</returns>
        public virtual async Task<User> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => (user.UserName == userNameOrEmailAddress || user.EmailAddress == userNameOrEmailAddress)
                );
        }

        /// <summary>
        /// Tries to find a user with user name or email address.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <returns>User or null</returns>
        [UnitOfWork]
        public virtual async Task<User> FindByNameOrEmailAsync(int? tenantId, string userNameOrEmailAddress)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await _userRepository.FirstOrDefaultAsync(
                    user =>
                        user.TenantId == tenantId &&
                        (user.UserName == userNameOrEmailAddress || user.EmailAddress == userNameOrEmailAddress)
                    );
            }
        }

        public virtual Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public virtual Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.Password);
        }

        public virtual Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }

        public virtual Task SetEmailAsync(User user, string email)
        {
            user.EmailAddress = email;
            return Task.FromResult(0);
        }

        public virtual Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult(user.EmailAddress);
        }

        public virtual Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task.FromResult(user.IsEmailConfirmed);
        }

        public virtual Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            user.IsEmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public virtual async Task AddLoginAsync(User user, UserLoginInfo login)
        {
            await _userLoginRepository.InsertAsync(
                new UserLogin
                {
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    UserId = user.Id
                });
        }

        public virtual async Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            await _userLoginRepository.DeleteAsync(
                ul => ul.UserId == user.Id &&
                      ul.LoginProvider == login.LoginProvider &&
                      ul.ProviderKey == login.ProviderKey
                );
        }

        public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            return (await _userLoginRepository.GetAllListAsync(ul => ul.UserId == user.Id))
                .Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey))
                .ToList();
        }

        public virtual async Task<User> FindAsync(UserLoginInfo login)
        {
            var userLogin = await _userLoginRepository.FirstOrDefaultAsync(
                ul => ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey
                );

            if (userLogin == null)
            {
                return null;
            }

            return await _userRepository.FirstOrDefaultAsync(u => u.Id == userLogin.UserId);
        }

        [UnitOfWork]
        public virtual Task<List<User>> FindAllAsync(UserLoginInfo login)
        {
            var query = from userLogin in _userLoginRepository.GetAll()
                        join user in _userRepository.GetAll() on userLogin.UserId equals user.Id
                        where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                        select user;

            return Task.FromResult(query.ToList());
        }

        public virtual Task<User> FindAsync(int? tenantId, UserLoginInfo login)
        {
            var query = from userLogin in _userLoginRepository.GetAll()
                join user in _userRepository.GetAll() on userLogin.UserId equals user.Id
                where user.TenantId == tenantId && userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                select user;

            return Task.FromResult(query.FirstOrDefault());
        }

        public virtual async Task AddToRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName);
            await _userRoleRepository.InsertAsync(
                new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
        }

        public virtual async Task RemoveFromRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName);
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
            if (userRole == null)
            {
                return;
            }

            await _userRoleRepository.DeleteAsync(userRole);
        }

        public virtual Task<IList<string>> GetRolesAsync(User user)
        {
            //TODO: This is not implemented as async.
            var roleNames = _userRoleRepository.Query(userRoles => (from userRole in userRoles
                join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                where userRole.UserId == user.Id
                select role.Name).ToList());

            return Task.FromResult<IList<string>>(roleNames);
        }

        public virtual async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName);
            return await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id) != null;
        }

        public virtual IQueryable<User> Users
        {
            get { return _userRepository.GetAll(); }
        }

        public virtual async Task AddPermissionAsync(User user, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(user, permissionGrant))
            {
                return;
            }

            await _userPermissionSettingRepository.InsertAsync(
                new UserPermissionSetting
                {
                    UserId = user.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }

        public virtual async Task RemovePermissionAsync(User user, PermissionGrantInfo permissionGrant)
        {
            await _userPermissionSettingRepository.DeleteAsync(
                permissionSetting => permissionSetting.UserId == user.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
                );
        }

        public virtual async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(User user)
        {
            return (await _userPermissionSettingRepository.GetAllListAsync(p => p.UserId == user.Id))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }

        public virtual async Task<bool> HasPermissionAsync(User user, PermissionGrantInfo permissionGrant)
        {
            return await _userPermissionSettingRepository.FirstOrDefaultAsync(
                p => p.UserId == user.Id &&
                     p.Name == permissionGrant.Name &&
                     p.IsGranted == permissionGrant.IsGranted
                ) != null;
        }

        public virtual async Task RemoveAllPermissionSettingsAsync(User user)
        {
            await _userPermissionSettingRepository.DeleteAsync(s => s.UserId == user.Id);
        }

        public virtual void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
