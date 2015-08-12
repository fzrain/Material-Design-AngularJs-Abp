using Fzrain.Common.Application.Services;
using Fzrain.Users.Dto;

namespace Fzrain.Users
{
    public interface IUserAppService : ICrudAppService<UserDto,long>
    {
        
    }
}
