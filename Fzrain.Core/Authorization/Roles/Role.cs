using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Fzrain.Authorization.Permissions;
using Fzrain.Authorization.Users;
using Microsoft.AspNet.Identity;

namespace Fzrain.Authorization.Roles
{
   
  
    public class Role : FullAuditedEntity<int, User>, IRole<int>, IMayHaveTenant     
    {
             
        public  int? TenantId { get; set; }     
        public  string Name { get; set; }        
        public  bool IsStatic { get; set; }
        public  bool IsDefault { get; set; }
        public  ICollection<PermissionSetting> Permissions { get; set; } = new List<PermissionSetting>();
      //  public virtual ICollection<User> Users { get; set; } = new List<User>();


    }
}
