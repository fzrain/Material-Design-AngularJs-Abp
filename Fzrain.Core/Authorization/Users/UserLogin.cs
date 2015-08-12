using Abp.Domain.Entities;

namespace Fzrain.Authorization.Users
{
    /// <summary>
    /// Used to store a User Login for external Login services.
    /// </summary>
    public class UserLogin : Entity<long>
    {
     
        public virtual long UserId { get; set; }    
        public virtual string LoginProvider { get; set; }     
        public virtual string ProviderKey { get; set; }
    }
}
