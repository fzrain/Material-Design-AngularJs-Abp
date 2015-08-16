using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Fzrain.Users.Dto
{
    public class UserSelectedInput : IPagedResultRequest
    {
        [Range(0, 1000)]
        public int MaxResultCount { get; set; }
        public int SkipCount { get; set; }
        
    }
}
