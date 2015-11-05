using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

        [AbpAuthorize("Administration.UserManager.Read")]
        public PagedResultOutput<UserListDto> GetUsers(UserQueryInput input)
        {
            return new PagedResultOutput<UserListDto>
            {
                TotalCount = userManager.Users.Count(),
                Items = userManager.Users.OrderByDescending(u => u.CreationTime).PageBy(input).ToList().MapTo<List<UserListDto>>()

            };
        }
        [AbpAuthorize("Administration.UserManager.Edit")]
        public async Task<UserEditOutput> GetUserForEdit(NullableIdInput<long> input)
        {
            User user = new User();
            IList<string> roleNames;
            if (input.Id.HasValue)
            {
                user = await userManager.GetUserByIdAsync((long)input.Id);
                roleNames = await userManager.GetRolesAsync(user.Id);
            }
            else
            {
                roleNames = roleManager.Roles.Where(r => r.IsDefault).Select(r => r.Name).ToList();
            }
            var dto = user.MapTo<UserEditOutput>();
            dto.RoleInfos = new List<dynamic>();
            foreach (Role role in roleManager.Roles.ToList())
            {
                dto.RoleInfos.Add(new
                {
                    role.Name,
                    IsAssigned = roleNames.Contains(role.Name)
                });
            }
            dto.CanChangeUserName = dto.UserName != "admin";
            return dto;
        }
        [AbpAuthorize("Administration.UserManager.Edit", "Administration.UserManager.Create")]
        public async Task AddOrUpdate(UserEditInput userEditInput)
        {           
            var userId = userEditInput.Id;         
            var user = userId.HasValue ? await userManager.GetUserByIdAsync((long) userId) : new User();
              await userManager.SetRoles(user, userEditInput.Roles);
            if (userEditInput.SetDefaultPassword)
            {
                userEditInput.Password = "111111";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(userEditInput.Password))
                {
                    throw new UserFriendlyException("密码不能为空！");
                }
                if (!userEditInput.Password.Equals(userEditInput.PasswordRepeat))
                {
                    throw new UserFriendlyException("2次密码不匹配！");
                }
               
            }         
            user.Name = userEditInput.Name;
            user.UserName = userEditInput.UserName;
            user.MobilePhone = userEditInput.MobilePhone;         
            user.EmailAddress = userEditInput.EmailAddress;
            user.IsActive = userEditInput.IsActive;
            IdentityResult result;
            if (userEditInput.Id.HasValue)
            {
                result = await userManager.UpdateAsync(user);
            }
            else
            {
                result = await userManager.CreateAsync(user,userEditInput.Password);
            }
            if (userEditInput.SendActivationEmail)
            {
                
                // ReSharper disable once PossibleInvalidOperationException
                var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync((long)userId);
                
                string callbackUrl = "http://localhost:6234/Account/ConfirmEmail?userId="+userId+"&conformCode="+ HttpUtility.UrlEncode(emailConfirmToken);
                await userManager.SendEmailAsync((long)userId, "账号激活", "确定激活账号 <a href=\"" + callbackUrl + "\">确认</a>");
      
            }
            if (!result.Succeeded)
            {
                throw new UserFriendlyException(result.Errors.FirstOrDefault());
            }
        }
        [AbpAuthorize("Administration.UserManager.Delete")]       
        public async Task Delete(IdInput<long> input)
        {
            var user = await userManager.GetUserByIdAsync(input.Id);
            await userManager.DeleteAsync(user);
        }
        [AbpAuthorize("Administration.UserManager.Permission")]
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
        [AbpAuthorize("Administration.UserManager.Permission")]
        public async Task UpdateUserPermission(UserPermissionInput input)
        {
            var user = await userManager.GetUserByIdAsync(input.Id);
            var permissions = permissionManager.GetAllPermissions().Where(p => input.Permissions.Contains(p.Name));
            await userManager.SetGrantedPermissionsAsync(user, permissions);
        }
        [AbpAuthorize("Administration.UserManager.Permission")]
        public async  Task ResetUserSpecificPermissions(IdInput<long> input)
        {
            var user =await userManager.GetUserByIdAsync(input.Id);
            await userManager.ResetAllPermissionsAsync(user);         
        }
    }
}