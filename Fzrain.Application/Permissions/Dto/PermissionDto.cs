using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fzrain.Authorization.Permissions;

namespace Fzrain.Permissions.Dto
{
    [AutoMapTo(typeof(PermissionInfo))]
    [AutoMapFrom(typeof(PermissionInfo))]
    public class PermissionDto: CreationAuditedEntityDto
    {
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool IsGrantedByDefault { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
    }
}
