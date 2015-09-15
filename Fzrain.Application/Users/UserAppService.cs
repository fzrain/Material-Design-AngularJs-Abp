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
        #region private field
        private readonly UserManager userManager;
        private readonly RoleManager roleManager;
        private readonly IRepository<User, long> userRepository;
        private readonly IPermissionManager permissionManager;
        #endregion

        #region ctor
        public UserAppService(UserManager userManager, RoleManager roleManager, IRepository<User, long> userRepository, IPermissionManager permissionManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userRepository = userRepository;
            this.permissionManager = permissionManager;
        }
        #endregion

        [AbpAuthorize("Administration.UserManger.Read")]
        public PagedResultOutput<UserDto> GetUsers(UserQueryInput input)
        {
            return new PagedResultOutput<UserDto>
            {
                TotalCount = userManager.Users.Count(),
                Items = userManager.Users.OrderByDescending(u => u.CreationTime).PageBy(input).ToList().MapTo<List<UserDto>>()

            };
        }

        public async Task<UserDto> GetUserForEdit(IdInput<long?> input)
        {
            User user = new User();
            if (input.Id.HasValue)
            {
                user = await userManager.GetUserByIdAsync((long)input.Id);
            }
            var userDto = user.MapTo<UserDto>();
            userDto.Roles = roleManager.Roles.MapTo<List<RoleDto>>();
            foreach (RoleDto role in userDto.Roles)
            {
                if (user.Roles.Any(r => r.Id == role.Id))
                {
                    role.IsDefault = true;
                }
            }
            return userDto;
        }

        public async Task AddOrUpdate(UserEditInput userEditDto)
        {
            var roleIds = userEditDto.Roles.Where(r => r.IsDefault).Select(r => r.Id);
            var user = userEditDto.Id.HasValue ? userManager.Users.Where(u => u.Id == userEditDto.Id).IncludeProperties(u => u.Roles).FirstOrDefault() : new User();
            if (user == null)
            {
                return;
            }
            user.Name = userEditDto.Name;
            user.UserName = userEditDto.UserName;
            user.MobilePhone = userEditDto.MobilePhone;
            user.TenantId = AbpSession.TenantId;
            user.Password = string.IsNullOrWhiteSpace(userEditDto.Password)?"11111": userEditDto.Password;
            user.EmailAddress = userEditDto.EmailAddress;
            user.Roles = roleManager.Roles.Where(r => roleIds.Contains(r.Id)).ToList();
            await userManager.CreateAsync(user);
            await userRepository.InsertOrUpdateAsync(user);


        }

        public async  Task Delete(IdInput<long> input)
        {
           var user=await userManager.GetUserByIdAsync(input.Id);
           await userManager.DeleteAsync(user);
        }

        public async Task<dynamic> GetUserPermissions(IdInput<long> input)
        {
            var user = await userManager.GetUserByIdAsync(input.Id);
            var permissions = await userManager.GetGrantedPermissionsAsync(user);
            List<dynamic> list = new List<dynamic>();
            foreach (Permission permission in permissionManager.GetAllPermissions())
            {
                var data = new
                {
                    permission.Name,
                    DisplayName = permission.DisplayName.Localize(),
                    ParentName = permission.Parent == null ? "无" : permission.Parent.Name,
                    IsGrantedByDefault = permissions.Contains(permission) || permission.IsGrantedByDefault
                };
                list.Add(data);
            }
            return list;
        }

        public async Task UpdateUserPermission(UserPermissionInput input)
        {
            var user = await userManager.GetUserByIdAsync(input.Id);
            var permissions = permissionManager.GetAllPermissions().Where(p => input.Permissions.Contains(p.Name));
            await userManager.SetGrantedPermissionsAsync(user, permissions);
        }
    }
}