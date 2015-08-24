using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
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

       public PagedResultOutput<RoleDto> GetRoles(GetRolesInput input)
       {
             return new PagedResultOutput<RoleDto>
                   {
                       Items = roleManager.Roles.OrderByDescending(r=>r.CreationTime).PageBy(input).ToList().MapTo<List<RoleDto>>(),
                       TotalCount =roleManager.Roles.Count()
                   };
          
       }

       public Task<IdentityResult> AddRole(RoleDto role)
       {
         return  roleManager.CreateAsync(new Role
         {
             IsDefault=role.IsDefault,
             Name =role.Name,
             DisplayName =role.Name
         });     
       }
   }
}   
