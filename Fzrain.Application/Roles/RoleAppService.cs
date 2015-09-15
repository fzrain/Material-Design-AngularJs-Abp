using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fzrain.Authorization.Permissions;
using Fzrain.Authorization.Roles;
using Fzrain.Roles.Dto;
using Newtonsoft.Json.Linq;

namespace Fzrain.Roles
{
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly RoleManager roleManager;
        private readonly IRepository<Role> roleRepository;
        private readonly IRepository<PermissionInfo> permissionRepository;
        private readonly IRepository<PermissionSetting, long> permissionSettingRepository;
        private readonly IPermissionManager permissionManager;



        public RoleAppService(RoleManager roleManager, IRepository<Role> roleRepository, IRepository<PermissionInfo> permissionRepository, IRepository<PermissionSetting, long> permissionSettingRepository, IPermissionManager permissionManager)
        {
            this.roleManager = roleManager;
            this.roleRepository = roleRepository;
            this.permissionRepository = permissionRepository;
            this.permissionSettingRepository = permissionSettingRepository;
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
            Role role = new Role
            {
                Id = roleDto.Id,
                Name = roleDto.Name,
                IsDefault = roleDto.IsDefault,
                TenantId = AbpSession.TenantId
            };
            var grantedPermissions = permissionManager.GetAllPermissions().Where(p => ((JArray)roleDto.Permissions).ToObject<List<string>>().Contains(p.Name)).ToList();
            await roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            await roleRepository.InsertOrUpdateAsync(role);
        }

        public async Task<EditRoleDto> GetById(IdInput<int?> input)
        {
            Role role = new Role();
            if (input.Id.HasValue)
            {
                 role = await roleManager.GetRoleByIdAsync((int)input.Id);
            }          
            var roleEditDto = role.MapTo<EditRoleDto>();
            var permissionNames = permissionSettingRepository.GetAll().Where(p => p.RoleId == role.Id).Select(p => p.Name).ToList();
            var permissionInfos = permissionRepository.GetAllList().Select(p => new { p.Id, p.Name, p.DisplayName, p.ParentName, IsGrantedByDefault = permissionNames.Contains(p.Name) || p.IsGrantedByDefault });
            roleEditDto.Permissions = permissionInfos;
            return roleEditDto;
        }

        public async Task Delete(IdInput input)
        {
            await roleRepository.DeleteAsync(input.Id);
        }
    }
}
