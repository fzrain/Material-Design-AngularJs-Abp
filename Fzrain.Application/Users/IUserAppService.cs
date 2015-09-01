using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fzrain.Common.Application.Dtos;
using Fzrain.Common.Application.Services;
using Fzrain.Users.Dto;

namespace Fzrain.Users
{
    public interface IUserAppService : IApplicationService
    {
        PagedResultOutput<UserDto> GetUsers(UserQueryInput input);
        Task<UserDto> GetUserForEdit(IdInput<long?> input);
        Task AddOrUpdate(UserEditInput userEditDto);
    }
}
