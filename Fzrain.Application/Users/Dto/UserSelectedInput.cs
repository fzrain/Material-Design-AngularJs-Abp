using Abp.Application.Services.Dto;

namespace Fzrain.Users.Dto
{
    public class UserSelectedInput : IPagedResultRequest
    {
        public int MaxResultCount { get; set; }
        public int SkipCount { get; set; }
    }
}
