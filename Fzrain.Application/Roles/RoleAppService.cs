using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fzrain.Authorization.Roles;
using Fzrain.Roles.Dto;
using Microsoft.AspNet.Identity;

namespace Fzrain.Roles
{
   public  class RoleAppService:ApplicationService,IRoleAppService
   {
       private readonly RoleManager roleManager;

       public RoleAppService(RoleManager roleManager)
       {
           this.roleManager = roleManager;
       }

       public PagedResultOutput<RoleDto> GetRoles()
       {
             return new PagedResultOutput<RoleDto>
                   {
                       Items = roleManager.Roles.ToList().MapTo<List<RoleDto>>()
                   };
          
       }

       public Task<IdentityResult> AddRole(RoleDto role)
       {
         return  roleManager.CreateAsync(role.MapTo<Role>());     
       }
   }
}
