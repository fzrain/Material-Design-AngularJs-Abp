using System.Threading.Tasks;
using Fzrain.MultiTenancy;

namespace Fzrain.Authorization.Users
{
    /// <summary>
    /// This is a helper base class to easily update <see>
    ///         <cref>IExternalAuthenticationSource{TTenant,TUser}</cref>
    ///     </see>
    ///     .
    /// Implements some methods as default but you can override all methods.
    /// </summary>
    public abstract class DefaultExternalAuthenticationSource : IExternalAuthenticationSource

    {

        public abstract string Name { get; }
        public abstract Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, Tenant tenant);
        public virtual Task<User> CreateUserAsync(string userNameOrEmailAddress, Tenant tenant)
        {
            return Task.FromResult(
                new User
                {
                    UserName = userNameOrEmailAddress,
                    Name = userNameOrEmailAddress,                  
                    EmailAddress = userNameOrEmailAddress,
                    IsEmailConfirmed = true,
                    IsActive = true
                });
        }

        public virtual Task UpdateUserAsync(User user, Tenant tenant)
        {
            return Task.FromResult(0);
        }
    }
}