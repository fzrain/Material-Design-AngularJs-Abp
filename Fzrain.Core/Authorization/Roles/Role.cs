using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Fzrain.Authorization.Permissions;
using Fzrain.Authorization.Users;
using Fzrain.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace Fzrain.Authorization.Roles
{
   
  
    public class Role : FullAuditedEntity<int, User>, IRole<int>, IMayHaveTenant     
    {
             
        public virtual int? TenantId { get; set; }     
        public virtual string Name { get; set; }        
        public virtual bool IsStatic { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual ICollection<PermissionSetting> Permissions { get; set; } = new List<PermissionSetting>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();


    }
}
