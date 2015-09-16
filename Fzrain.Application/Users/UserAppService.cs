using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.UI;
using Fzrain.Authorization.Roles;
using Fzrain.Authorization.Users;
using Fzrain.Users.Dto;
using Microsoft.AspNet.Identity;

namespace Fzrain.Users
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        #region private field
        private readonly UserManager userManager;
        private readonly RoleManager roleManager;
        private readonly IPermissionManager permissionManager;
        #endregion

        #region ctor
        public UserAppService(UserManager userManager, RoleManager roleManager,  IPermissionManager permissionManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.permissionManager = permissionManager;
        }
        #endregion

        [AbpAuthorize("Administration.UserManger.Read")]
        public PagedResultOutput<UserListDto> GetUsers(UserQueryInput input)
        {
            return new PagedResultOutput<UserListDto>
            {
                TotalCount = userManager.Users.Count(),
                Items = userManager.Users.OrderByDescending(u => u.CreationTime).PageBy(input).ToList().MapTo<List<UserListDto>>()

            };
        }

        public async Task<UserEditOutput> GetUserForEdit(NullableIdInput<long> input)
        {
            User user = new User();
            IList<string> roleNames = new List<string>();
            if (input.Id.HasValue)
            {
                user = await userManager.GetUserByIdAsync((long)input.Id);
                roleNames = await userManager.GetRolesAsync(user.Id);
            }
            var dto = user.MapTo<UserEditOutput>();
            dto.RoleInfos = new List<dynamic>();
            foreach (Role role in roleManager.Roles.ToList())
            {
                dto.RoleInfos.Add(new
                {
                    role.Name,
                    IsAssigned = roleNames.Contains(role.Name) || role.IsDefault
                });
            }
            return dto;
        }

        public async Task AddOrUpdate(UserEditInput userEditInput)
        {
            var userId = userEditInput.Id;
            List<string> roleNames = new List<string>();
            foreach (var roleInfo in userEditInput.RoleInfos)
            {
                if (roleInfo.IsAssigned)
                {
                    roleNames.Add(roleInfo.Name);
                }
            }
           // var roleNames = userEditInput.RoleInfos.Where(r => r.IsAssigned).Select(r => r.Name);
            var user = userId.HasValue ? await userManager.GetUserByIdAsync((long) userId) : new User();
            //   await userManager.SetRoles(user, (string[]) roleNames);
            user.Name = userEditInput.Name;
            user.UserName = userEditInput.UserName;
            user.MobilePhone = userEditInput.MobilePhone;
            user.TenantId = AbpSession.TenantId;
            user.EmailAddress = userEditInput.EmailAddress;
            IdentityResult result;
            if (userEditInput.Id.HasValue)
            {
                result = await userManager.UpdateAsync(user);
            }
            else
            {
                result = await userManager.CreateAsync(user, string.IsNullOrWhiteSpace(userEditInput.Password) ? "111111" : userEditInput.Password);
            }          
            if (!result.Succeeded)
            {
                throw new UserFriendlyException(result.Errors.FirstOrDefault());
            }
        }

        public async Task Delete(IdInput<long> input)
        {
            var user = await userManager.GetUserByIdAsync(input.Id);
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