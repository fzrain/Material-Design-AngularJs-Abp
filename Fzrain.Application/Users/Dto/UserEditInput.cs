using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fzrain.Authorization.Permissions;
using Fzrain.Authorization.Roles;
using Fzrain.Authorization.Users;
using Fzrain.Roles.Dto;

namespace Fzrain.Users.Dto
{   [AutoMapTo(typeof(User))]
    public  class UserEditInput:IdInput<long>
{   
        public virtual string Name { get; set; } 
        public virtual string MobilePhone { get; set; }     
        public virtual string UserName { get; set; }   
        public virtual string Password { get; set; } 
        public virtual string EmailAddress { get; set; }   
        public virtual bool IsEmailConfirmed { get; set; }
        public virtual List<RoleDto> Roles { get; set; } 
       // public virtual ICollection<PermissionSetting> Permissions { get; set; } = new List<PermissionSetting>();
    }
}
