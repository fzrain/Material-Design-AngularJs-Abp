using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fzrain.Common;
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
         [AbpAuthorize("Administration.Permission.Create", "Administration.Permission.Edit")]
        public async Task AddOrUpdate(PermissionDto permission)
        {
            await permissionRepository.InsertOrUpdateAsync(permission.MapTo<PermissionInfo>());
        }
         [AbpAuthorize("Administration.Permission.Edit")]
        public async  Task<PermissionDto> GetById(IdInput input)
        {
            var permission= await  permissionRepository.GetAsync(input.Id);
            return permission.MapTo<PermissionDto>();
        }
        [AbpAuthorize("Administration.Permission.Delete")]
        public async Task Delete(IdInput input)
        {
            await permissionRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize("Administration.Permission.Read")]
        public async Task<PagedResultOutput<PermissionDto>> GetPermissions(PermissionQueryInput input)
        {
            int totalCount;
            return new PagedResultOutput<PermissionDto>
            {
                TotalCount = await permissionRepository.CountAsync(),
                Items = permissionRepository.GetAll().FilterBy(input, out totalCount).OrderByDescending(p => p.Id).PageBy(input).ToList().MapTo<List<PermissionDto>>()
            };
        }
        public List<string> GetPermissionNames()
        {
            return permissionRepository.GetAll().Select(p => p.Name).ToList();
        }
    }
}
