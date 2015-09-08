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
using Fzrain.Roles.Dto;
using Fzrain.Users.Dto;

namespace Fzrain.Users
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly UserManager userManager;
        private readonly RoleManager roleManager;
        private readonly IRepository<User, long> userRepository;
        public UserAppService(UserManager userManager, RoleManager roleManager, IRepository<User, long> userRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userRepository = userRepository;
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
            User user = new User ();
            if (input.Id.HasValue)
            {
                 user = await userManager.GetUserByIdAsync((long)input.Id);             
            }                           
            var userDto= user.MapTo<UserDto>();
            userDto.Roles = roleManager.Roles.MapTo<List<RoleDto>>();
            foreach (RoleDto role in userDto.Roles)
            {
                if (user.Roles.Any(r => r.Id==role.Id))
                {
                    role.IsDefault = true;
                }
            }
            return userDto;
        }

        public async  Task AddOrUpdate(UserEditInput userEditDto)
        {
            var roleIds=  userEditDto.Roles.Where(r => r.IsDefault).Select(r=>r.Id);           
            User user = new User
            {
                Id = userEditDto.Id,
                Name = userEditDto.Name,
                UserName = userEditDto.UserName,
                MobilePhone = userEditDto.MobilePhone,
                TenantId = AbpSession.TenantId,
                Password = userEditDto.Password,
                EmailAddress = userEditDto.EmailAddress,
                Roles = roleManager.Roles.Where(r => roleIds.Contains(r.Id)).ToList()
            };
            await userRepository.UpdateAsync(user);
            //await userManager.UpdateAsync(user);
        }
    }
}