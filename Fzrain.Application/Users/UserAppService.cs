using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
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
            var user = userEditDto.Id.HasValue ? userManager.Users.Where(u => u.Id == userEditDto.Id).IncludeProperties(u=>u.Roles).FirstOrDefault() : new User();           
            if (user == null)
            {
                return;
            }
            user.Name = userEditDto.Name;
            user.UserName = userEditDto.UserName;
            user.MobilePhone = userEditDto.MobilePhone;
            user.TenantId = AbpSession.TenantId;
            user.Password = userEditDto.Password;
            user.EmailAddress = userEditDto.EmailAddress;
            user.Roles = roleManager.Roles.Where(r => roleIds.Contains(r.Id)).ToList();

            await userRepository.InsertOrUpdateAsync(user);
         
                   
        }
    }
}