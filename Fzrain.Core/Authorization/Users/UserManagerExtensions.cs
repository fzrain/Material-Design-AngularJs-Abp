using System;
using Abp.Threading;

namespace Fzrain.Authorization.Users
{
    /// <summary>
    /// Extension methods for <see cref="UserManager"/>.
    /// </summary>
    public static class UserManagerExtensions
    {
        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="manager">User manager</param>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public static bool IsGranted(UserManager manager, long userId, string permissionName)
         
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            return AsyncHelper.RunSync(() => manager.IsGrantedAsync(userId, permissionName));
        }

        public static UserManager.LoginResult Login(UserManager manager, string userNameOrEmailAddress, string plainPassword, string tenancyName = null)
         
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            return AsyncHelper.RunSync(() => manager.LoginAsync(userNameOrEmailAddress, plainPassword, tenancyName));
        }
    }
}