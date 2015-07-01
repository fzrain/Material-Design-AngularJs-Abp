using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fzrain.Users.Dto;

namespace Fzrain.Users
{
    public interface IUserAppService : IApplicationService
    {
        PagedResultOutput<UserDto> GetUsers();
    }
}
