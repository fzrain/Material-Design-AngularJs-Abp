using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Fzrain.Authorization.Roles;
using Fzrain.Roles.Dto;
using Newtonsoft.Json.Linq;

namespace Fzrain.Roles
{
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly RoleManager roleManager;
        private readonly IPermissionManager permissionManager;

        public RoleAppService(RoleManager roleManager,  IPermissionManager permissionManager)
        {
            this.roleManager = roleManager;
            this.permissionManager = permissionManager;
        }

        public PagedResultOutput<RoleDto> GetRoles(RoleQueryInput input)
        {
            return new PagedResultOutput<RoleDto>
            {
                Items = roleManager.Roles.OrderByDescending(r => r.CreationTime).PageBy(input).ToList().MapTo<List<RoleDto>>(),
                TotalCount = roleManager.Roles.Count()
            };
        }


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
                DisplayName = p.DisplayName.Localize(),
                ParentName = p.Parent == null ? "无" : p.Parent.Name,
                IsGrantedByDefault = permissions.Contains(p) || p.IsGrantedByDefault
            });
            roleEditDto.Permissions = permissionInfos;
            return roleEditDto;
        }

        public async Task Delete(IdInput input)
        {
           var role =await roleManager.GetRoleByIdAsync(input.Id);
            await roleManager.DeleteAsync(role);
        }
    }
}
