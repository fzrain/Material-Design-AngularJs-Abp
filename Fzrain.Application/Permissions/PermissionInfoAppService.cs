using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fzrain.Authorization.Permissions;
using Fzrain.Permissions.Dto;

namespace Fzrain.Permissions
{
    public class PermissionInfoAppService : ApplicationService, IPermissionInfoAppService
    {
        private readonly IRepository<PermissionInfo> permissionRepository;

        public PermissionInfoAppService(IRepository<PermissionInfo> permissionRepository)
        {
            this.permissionRepository = permissionRepository;
        }

        public async Task AddOrUpdate(PermissionDto permission)
        {

            await permissionRepository.InsertOrUpdateAsync(new PermissionInfo
            {
                Id =permission.Id,
                IsGrantedByDefault = permission.IsGrantedByDefault,
                Description = permission.Description,
                Name = permission.Name,
                DisplayName = permission.DisplayName,
                ParentName = permission.ParentName
            });
        }

        public async  Task<PermissionDto> GetById(IdInput input)
        {
            var permission= await  permissionRepository.GetAsync(input.Id);
            return permission.MapTo<PermissionDto>();
        }


        public async Task Delete(IdInput input)
        {
            await permissionRepository.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<PermissionDto>> GetPermissions(PermissionQueryInput input)
        {
            return new PagedResultOutput<PermissionDto>
            {
                TotalCount = await permissionRepository.CountAsync(),
                Items = permissionRepository.GetAll().OrderByDescending(p => p.Id).PageBy(input).ToList().MapTo<List<PermissionDto>>()

            };
        }

        public List<string> GetPermissionNames()
        {
            return permissionRepository.GetAll().Select(p => p.Name).ToList();
        }
    }
}
