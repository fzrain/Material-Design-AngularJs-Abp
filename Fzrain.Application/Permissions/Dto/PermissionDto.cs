using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Fzrain.Permissions.Dto
{
    [AutoMapTo(typeof(PermissionInfo))]
    [AutoMapFrom(typeof(PermissionInfo))]
    public class PermissionDto: CreationAuditedEntityDto<int?>
    {
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool? IsGrantedByDefault { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
    }
}
