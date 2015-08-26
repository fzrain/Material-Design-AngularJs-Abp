using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fzrain.Authorization.Permissions;
using Fzrain.Permissions.Dto;

namespace Fzrain.Permissions
{
    public  interface IPermissionInfoAppService: IApplicationService
    {
        Task AddOrUpdate(PermissionDto permission);
        Task<PermissionDto> GetById(IdInput input);
        Task Delete(IdInput input);
        Task<PagedResultOutput<PermissionDto>> GetPermissions(PermissionQueryInput input);
        List<string> GetPermissionNames();
    }
}
