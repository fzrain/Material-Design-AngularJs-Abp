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
        public virtual int? TenantId { get; set; }            
        public virtual string AuthenticationSource { get; set; }     
        public virtual string Name { get; set; }
        public string  MobilePhone { get; set; } 
        public virtual string UserName { get; set; }  
        public virtual string Password { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual bool IsEmailConfirmed { get; set; }     
        public virtual string EmailConfirmationCode { get; set; }    
        public virtual string PasswordResetCode { get; set; }
        public virtual DateTime? LastLoginTime { get; set; }
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