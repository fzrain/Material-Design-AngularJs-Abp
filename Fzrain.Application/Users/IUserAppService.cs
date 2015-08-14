using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fzrain.Common.Application.Services;
using Fzrain.Users.Dto;

namespace Fzrain.Users
{
    public interface IUserAppService : IApplicationService
    {
        PagedResultOutput<UserDto> GetUsers();       
    }
}
