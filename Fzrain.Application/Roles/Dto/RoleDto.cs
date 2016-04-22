using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fzrain.Authorization.Roles;

namespace Fzrain.Roles.Dto
{
    [AutoMapTo(typeof(Role))]
    [AutoMapFrom(typeof(Role))]
    public class RoleDto : CreationAuditedEntityDto<int?>
    {
        public string  Name { get; set; }
        public bool? IsStatic { get; set; }
        public bool? IsDefault { get; set; }
    }
}
