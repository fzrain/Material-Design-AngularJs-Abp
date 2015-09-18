using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Fzrain.Authorization.Permissions;
using Fzrain.Authorization.Roles;
using Fzrain.Runtime.Session;
using Microsoft.AspNet.Identity;
namespace Fzrain.Authorization.Users
{
    /// <summary>
    /// Implements 'User Store' of ASP.NET Identity Framework.
    /// </summary>
    public  class UserStore :
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
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<PermissionSetting, long> _permissionSettingRepository;      
        private readonly IUnitOfWorkManager _unitOfWorkManager;
       
        /// <summary>
        /// Constructor.
        /// </summary>
        public UserStore(
            IRepository<User,long> userRepository,
            IRepository<UserLogin,long> userLoginRepository,
            IRepository<Role> roleRepository,
            IRepository<PermissionSetting,long> permissionSettingRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _roleRepository = roleRepository;
            _unitOfWorkManager = unitOfWorkManager;
         

            _permissionSettingRepository = permissionSettingRepository;
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

     
        public virtual async Task<User> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => (user.UserName == userNameOrEmailAddress || user.EmailAddress == userNameOrEmailAddress)
                );
        }
      
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
            user.Roles.Add(role);
            await _userRepository.UpdateAsync(user);        
        }

        public virtual async Task RemoveFromRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName);
            user.Roles.Remove(role);
            await _userRepository.UpdateAsync(user);
          
        }

        public virtual async   Task<IList<string>> GetRolesAsync(User user)
        {
         //  var roleIds= _dbContext.Database.SqlQuery<int>("select Role_Id from User_R_Role where User_Id='" + user.Id + "'");
            return (await _userRepository.GetAsync(user.Id)).Roles.Select(r => r.Name).ToList();
        }

        public virtual async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName);
            return user.Roles.Contains(role);
        }

        public virtual IQueryable<User> Users => _userRepository.GetAll();

        public virtual async Task AddPermissionAsync(User user, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(user, permissionGrant))
            {
                return;
            }

            await _permissionSettingRepository.InsertAsync(
                new PermissionSetting
                {
                    UserId = user.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }

        public virtual async Task RemovePermissionAsync(User user, PermissionGrantInfo permissionGrant)
        {
            await _permissionSettingRepository.DeleteAsync(
                permissionSetting => permissionSetting.UserId == user.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
                );
        }

        public virtual async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(User user)
        {
            return (await _permissionSettingRepository.GetAllListAsync(p => p.UserId == user.Id))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }

        public virtual async Task<bool> HasPermissionAsync(User user, PermissionGrantInfo permissionGrant)
        {
            return await _permissionSettingRepository.FirstOrDefaultAsync(
                p => p.UserId == user.Id &&
                     p.Name == permissionGrant.Name &&
                     p.IsGranted == permissionGrant.IsGranted
                ) != null;
        }

        public virtual async Task RemoveAllPermissionSettingsAsync(User user)
        {
            await _permissionSettingRepository.DeleteAsync(s => s.UserId == user.Id);
        }

        public virtual void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
