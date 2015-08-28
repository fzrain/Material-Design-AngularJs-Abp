using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Fzrain.Authorization.Roles;

namespace Fzrain.Roles.Dto
{
    [AutoMapTo(typeof(Role))]
    [AutoMapFrom(typeof(Role))]
    public class RoleDto : FullAuditedEntity
    {
        public string  Name { get; set; }
        public bool IsStatic { get; set; }
        public bool IsDefault { get; set; }
    }
}
