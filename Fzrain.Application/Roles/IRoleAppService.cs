using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fzrain.Roles.Dto;

namespace Fzrain.Roles
{
   public  interface  IRoleAppService:IApplicationService
    {
       PagedResultOutput<RoleDto> GetRoles(RoleQueryInput input);    
       Task AddOrUpdate(EditRoleDto roleDto);
       Task<EditRoleDto> GetById(NullableIdInput input);
       Task Delete(IdInput input);
    }
}
