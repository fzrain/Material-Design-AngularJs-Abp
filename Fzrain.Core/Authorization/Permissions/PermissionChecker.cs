using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Fzrain.Authorization.Users;

namespace Fzrain.Authorization.Permissions
{
    /// <summary>
    /// Application should inherit this class to implement <see cref="IPermissionChecker"/>.
    /// </summary>
    public abstract class PermissionChecker : IPermissionChecker, ITransientDependency
     
    {
        private readonly UserManager _userManager;

        public ILogger Logger { get; set; }

        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PermissionChecker(UserManager userManager)
        {
            _userManager = userManager;

            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public virtual async Task<bool> IsGrantedAsync(string permissionName)
        {
            return AbpSession.UserId.HasValue && await _userManager.IsGrantedAsync(AbpSession.UserId.Value, permissionName);
        }

        public virtual async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await _userManager.IsGrantedAsync(userId, permissionName);
        }
    }
}
