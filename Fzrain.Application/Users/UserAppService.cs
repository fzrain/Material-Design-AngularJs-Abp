using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fzrain.Authorization.Users;
using Fzrain.Common.Application.Dtos;
using Fzrain.Common.Application.Services;
using Fzrain.Users.Dto;

namespace Fzrain.Users
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly UserManager userManager;
        public UserAppService(UserManager userManager)
        {
            this.userManager = userManager;
        }

        public PagedResultOutput<UserDto> GetUsers(UserSelectedInput input)
        {
            return new PagedResultOutput<UserDto>
            {
                TotalCount =userManager.Users.Count(),
                Items =userManager.Users.OrderByDescending(u=>u.CreationTime).PageBy(input).ToList().MapTo<List<UserDto>>()

            };
        }

        public async  Task<UserDto> GetDetail(IdInput<long> input)
        {
            var user = await userManager.GetUserByIdAsync(input.Id);
            return user.MapTo<UserDto>();
        }
    }
}