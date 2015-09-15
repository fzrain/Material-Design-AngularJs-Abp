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
       Task AddOrUpdate(EditRoleDto roleDto);
       Task<EditRoleDto> GetById(IdInput<int?> input);
       Task Delete(IdInput input);
    }
}
