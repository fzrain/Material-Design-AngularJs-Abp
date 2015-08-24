using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fzrain.Roles.Dto;
using Microsoft.AspNet.Identity;

namespace Fzrain.Roles
{
   public  interface  IRoleAppService:IApplicationService
    {
       PagedResultOutput<RoleDto> GetRoles(GetRolesInput input);
        Task<IdentityResult> AddRole(RoleDto role);

    }
}
