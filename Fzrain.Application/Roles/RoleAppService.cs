using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fzrain.Authorization.Roles;
using Fzrain.Roles.Dto;

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
    }
}
