using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fzrain.Authorization.Roles;

namespace Fzrain.Roles.Dto
{
    [AutoMapFrom(typeof (Role))]
    public class EditRoleDto : NullableIdInput
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public dynamic Permissions { get; set; }
    }
}
