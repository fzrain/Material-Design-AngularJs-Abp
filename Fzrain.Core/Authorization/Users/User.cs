using System;
using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;
using Fzrain.Authorization.Permissions;
using Fzrain.Configuration;
using Microsoft.AspNet.Identity;

namespace Fzrain.Authorization.Users
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User : FullAuditedEntity<long, User>, IUser<long>,IMayHaveTenant, IPassivable
    {

        public const string AdminUserName = "admin"; 
        public  int? TenantId { get; set; }            
        public  string AuthenticationSource { get; set; }     
        public  string Name { get; set; }
        public string  MobilePhone { get; set; } 
        public  string UserName { get; set; }  
        public  string Password { get; set; }
        public  string EmailAddress { get; set; }
        public  bool IsEmailConfirmed { get; set; }     
        public  string EmailConfirmationCode { get; set; }    
        public  string PasswordResetCode { get; set; }
        public  DateTime? LastLoginTime { get; set; }
        public  bool IsActive { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
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