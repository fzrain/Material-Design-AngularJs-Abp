using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fzrain.Authorization.Roles;
using Fzrain.Authorization.Users;
using Fzrain.Common.Application.Dtos;
using Fzrain.Common.Application.Services;
using Fzrain.Users.Dto;

namespace Fzrain.Users
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly UserManager userManager;
        private readonly RoleManager roleManager;
        public UserAppService(UserManager userManager, RoleManager roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public PagedResultOutput<UserDto> GetUsers(UserQueryInput input)
        {
            return new PagedResultOutput<UserDto>
            {
                TotalCount =userManager.Users.Count(),
                Items =userManager.Users.OrderByDescending(u=>u.CreationTime).PageBy(input).ToList().MapTo<List<UserDto>>()

            };
        }

        public async  Task<UserDto> GetUserForEdit(IdInput<long?> input)
        {
            User user = new User {Roles = roleManager.Roles.ToList()};
            if (input.Id.HasValue)
            {
                 user = await userManager.GetUserByIdAsync((long)input.Id);
               //  user.Roles = roleManager.Roles.ToList();
            }                  
            return user.MapTo<UserDto>();
        }

        public async  Task AddOrUpdate(UserEditInput userEditDto)
        {
            if (userEditDto.Id<=0)
            {
                await userManager.CreateAsync(userEditDto.MapTo<User>());
            }
            else
            {
               await userManager.UpdateAsync(userEditDto.MapTo<User>());
            }
        }
    }
}