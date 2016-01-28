using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Fzrain.Permissions.Dto
{
    public class PermissionQueryInput: IPagedResultRequest
    {
        [Range(0, 1000)]
        public int MaxResultCount { get; set; }
        public int SkipCount { get; set; }
    }
}
