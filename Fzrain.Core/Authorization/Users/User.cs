using System;
using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;
using Fzrain.Authorization.Permissions;
using Fzrain.Authorization.Roles;
using Fzrain.Configuration;
using Fzrain.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace Fzrain.Authorization.Users
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User : FullAuditedEntity<long, User>, IUser<long>,IMayHaveTenant, IPassivable
    {

        public const string AdminUserName = "admin";
        /// <summary>
        /// Tenant of this user.
        /// </summary>     
        public virtual Tenant Tenant { get; set; }

        /// <summary>
        /// Tenant Id of this user.
        /// </summary>
        public virtual int? TenantId { get; set; }
        
        /// <summary>
        /// Authorization source name.
        /// It's set to external authentication source name if created by an external source.
        /// Default: null.
        /// </summary>       
        public virtual string AuthenticationSource { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>      
        public virtual string Name { get; set; }

        /// <summary>
        /// Surname of the user.
        /// </summary>    
        public virtual string Surname { get; set; }

        /// <summary>
        /// User name.
        /// User name must be unique for it's tenant.
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Password of the user.
        /// </summary>
      
        public virtual string Password { get; set; }

        /// <summary>
        /// Email address of the user.
        /// Email address must be unique for it's tenant.
        /// </summary>
      
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// Is the <see cref="EmailAddress"/> confirmed.
        /// </summary>
        public virtual bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Confirmation code for email.
        /// </summary>      
        public virtual string EmailConfirmationCode { get; set; }

        /// <summary>
        /// ÷ÿ÷√√‹¬Î.
        /// It's not valid if it's null.
        /// It's for one usage and must be set to null after reset.
        /// </summary>    
        public virtual string PasswordResetCode { get; set; }

        /// <summary>
        /// The last time this user entered to the system.
        /// </summary>
        public virtual DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// Is this user active?
        /// If as user is not active, he/she can not use the application.
        /// </summary>
        public virtual bool IsActive { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public virtual ICollection<PermissionSetting> Permissions { get; set; } = new List<PermissionSetting>();
        public virtual ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
        public virtual ICollection<Setting> Settings { get; set; } = new List<Setting>();
    
        public virtual void SetNewPasswordResetCode()
        {
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(128);
        }

        public virtual void SetNewEmailConfirmationCode()
        {
            EmailConfirmationCode = Guid.NewGuid().ToString("N").Truncate(128);
        }

    }
}