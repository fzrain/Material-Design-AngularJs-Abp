using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;
using Fzrain.Authorization.Roles;
using Fzrain.Configuration;
using Fzrain.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace Fzrain.Authorization.Users
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User : FullAuditedEntity<long, User>, IUser<long>     
    {
        /// <summary>
        /// UserName of the admin.
        /// admin can not be deleted and UserName of the admin can not be changed.
        /// </summary>
        public const string AdminUserName = "admin";
       
   
        public const int MaxNameLength = 32;

     
     
     
        /// <summary>
        /// Maximum length of the <see cref="Password"/> without hashed.
        /// </summary>
        public const int MaxPlainPasswordLength = 32;


    
        /// <summary>
        /// Maximum length of the <see cref="AuthenticationSource"/> property.
        /// </summary>
        public const int MaxAuthenticationSourceLength = 64;

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
        [MaxLength(MaxAuthenticationSourceLength)]
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

        /// <summary>
        /// Role definitions for this user.
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// Permission definitions for this user.
        /// </summary>
        public virtual ICollection<UserPermissionSetting> Permissions { get; set; }

        /// <summary>
        /// Settings for this user.
        /// </summary>
        public virtual ICollection<Setting> Settings { get; set; }

     
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