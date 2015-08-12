using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fzrain.Roles.Dto;

namespace Fzrain.Roles
{
   public  interface  IRoleAppService:IApplicationService
    {
       PagedResultOutput<RoleDto> GetRoles();
     
    }
}
