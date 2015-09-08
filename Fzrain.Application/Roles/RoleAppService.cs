using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fzrain.Authorization.Permissions;
using Fzrain.Authorization.Roles;
using Fzrain.Roles.Dto;

namespace Fzrain.Roles
{
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly RoleManager roleManager;
        private readonly IRepository<Role> roleRepository;
        private readonly IRepository<PermissionInfo> permissionRepository;
        private readonly IRepository<PermissionSetting, long> permissionSettingRepository;



        public RoleAppService(RoleManager roleManager, IRepository<Role> roleRepository, IRepository<PermissionInfo> permissionRepository, IRepository<PermissionSetting, long> permissionSettingRepository)
        {
            this.roleManager = roleManager;
            this.roleRepository = roleRepository;
            this.permissionRepository = permissionRepository;
            this.permissionSettingRepository = permissionSettingRepository;
        }

        public PagedResultOutput<RoleDto> GetRoles(RoleQueryInput input)
        {
            return new PagedResultOutput<RoleDto>
            {
                Items = roleManager.Roles.OrderByDescending(r => r.CreationTime).PageBy(input).ToList().MapTo<List<RoleDto>>(),
                TotalCount = roleManager.Roles.Count()
            };
        }


        public async Task AddOrUpdate(RoleDto roleDto)
        {
            Role role = roleDto.MapTo<Role>();
            role.TenantId = AbpSession.TenantId;
            await roleRepository.InsertOrUpdateAsync(role);
        }

        public async Task<EditRoleDto> GetById(IdInput input)
        {
            var role = await roleManager.GetRoleByIdAsync(input.Id);
            var roleEditDto = role.MapTo<EditRoleDto>();
            var permissionNames = permissionSettingRepository.GetAll().Where(p => p.RoleId == role.Id).Select(p => p.Name).ToList();
            var permissionInfos = permissionRepository.GetAllList().Select(p=> new {p.Name,p.DisplayName,p.ParentName,p.IsGrantedByDefault});
            roleEditDto.Permissions = permissionInfos;
            foreach (var permissionInfo in roleEditDto.Permissions)
            {
                if (permissionNames.Contains(permissionInfo.Name))
                {
                    permissionInfo.IsGrantedByDefault = true;
                }
            }
            return roleEditDto;
        }

        public async Task Delete(IdInput input)
        {
            await roleRepository.DeleteAsync(input.Id);
        }
    }
}
