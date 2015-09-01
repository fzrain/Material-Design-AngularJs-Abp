using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fzrain.Roles.Dto;
using Microsoft.AspNet.Identity;

namespace Fzrain.Roles
{
   public  interface  IRoleAppService:IApplicationService
    {
       PagedResultOutput<RoleDto> GetRoles(RoleQueryInput input);
        dynamic GetAllRoleList();
       Task AddOrUpdate(RoleDto roleDto);
       Task<RoleDto> GetById(IdInput input);
       Task Delete(IdInput input);
    }
}
