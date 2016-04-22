using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fzrain.Users.Dto;
using Microsoft.AspNet.Identity;

namespace Fzrain.Users
{
    public interface IUserAppService : IApplicationService
    {
        PagedResultOutput<UserDto> GetUsers(UserQueryInput input);
        Task<UserEditOutput> GetUserForEdit(NullableIdInput<long> input);
        Task AddOrUpdate(UserEditInput userEditInput);
        Task Delete(IdInput<long> input);
        Task<dynamic> GetUserPermissions(IdInput<long> input);
        Task UpdateUserPermission(UserPermissionInput input);
        Task ResetUserSpecificPermissions(IdInput<long> input);
        Task<IdentityResult> ChangePassword(ChangePasswordInput input);
    }
}
