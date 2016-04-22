using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.Localization;
using Fzrain.Authorization.Roles;
using Fzrain.Roles.Dto;
using Newtonsoft.Json.Linq;
using Fzrain.Common;


namespace Fzrain.Roles
{
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly RoleManager roleManager;
        private readonly IPermissionManager permissionManager;
        private readonly ILocalizationContext localizationContext;

        public RoleAppService(RoleManager roleManager,  IPermissionManager permissionManager, ILocalizationContext localizationContext)
        {
            this.roleManager = roleManager;
            this.permissionManager = permissionManager;
            this.localizationContext = localizationContext;
        }
        [AbpAuthorize("Administration.Role.Read")]
        public PagedResultOutput<RoleDto> GetRoles(RoleQueryInput input)
        {
            int totalCount;
            return new PagedResultOutput<RoleDto>
            {             
                Items = roleManager.Roles.FilterBy(input, out totalCount).OrderByDescending(r => r.CreationTime).PageBy(input).ToList().MapTo<List<RoleDto>>(),
                TotalCount = roleManager.Roles.Count()
            };
        }

        [AbpAuthorize("Administration.Role.Create", "Administration.Role.Edit")]
        public async Task AddOrUpdate(EditRoleDto roleDto)
        {
            Role role = new Role();         
            if (roleDto.Id.HasValue)
            {
                role = await roleManager.GetRoleByIdAsync((int) roleDto.Id);             
            }
            role.Name = roleDto.Name;
            role.IsDefault = roleDto.IsDefault;
            var grantedPermissions = permissionManager.GetAllPermissions().Where(p => ((JArray)roleDto.Permissions).ToObject<List<string>>().Contains(p.Name)).ToList();
            await roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            if (roleDto.Id.HasValue)
                await roleManager.UpdateAsync(role);
            else                   
                await roleManager.CreateAsync(role);
        }
        [AbpAuthorize("Administration.Role.Edit")]
        public async Task<EditRoleDto> GetById(NullableIdInput input)
        {
            Role role = new Role();
            if (input.Id.HasValue)
            {
                 role = await roleManager.GetRoleByIdAsync((int)input.Id);
            }          
            var roleEditDto = role.MapTo<EditRoleDto>();
            var permissions=await roleManager.GetGrantedPermissionsAsync(role);
          
            var permissionInfos = permissionManager.GetAllPermissions().Select(p => new {
                p.Name,
                DisplayName = p.DisplayName.Localize(localizationContext),
                ParentName = p.Parent == null ? "无" : p.Parent.Name,
                IsGrantedByDefault = permissions.Contains(p) || p.IsGrantedByDefault
            });
            roleEditDto.Permissions = permissionInfos;
            return roleEditDto;
        }
        [AbpAuthorize("Administration.Role.Delete")]
        public async Task Delete(IdInput input)
        {
           var role =await roleManager.GetRoleByIdAsync(input.Id);
            await roleManager.DeleteAsync(role);
        }
    }
}
