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
   public  class PermissionInfoAppService: ApplicationService,IPermissionInfoAppService
   {
       private readonly IRepository<PermissionInfo> permissionRepository;

       public PermissionInfoAppService(IRepository<PermissionInfo> permissionRepository)
       {
           this.permissionRepository = permissionRepository;
       }

       public async Task Add(PermissionDto permission)
       {
        //   await  permissionRepository.InsertAsync(new PermissionInfo);
       }

       public async Task Update(PermissionDto permission)
       {
          //  await permissionRepository.UpdateAsync(permission);
        }

       public async Task Delete(IdInput input)
       {
           await permissionRepository.DeleteAsync(input.Id);
       }

       public async Task<PagedResultOutput<PermissionDto>> GetPermissions(PermissionQueryInput input)
       {
            return new PagedResultOutput<PermissionDto>
            {
                TotalCount =await permissionRepository.CountAsync(),
                Items = permissionRepository.GetAll().OrderByDescending(p=>p.Id).PageBy(input).ToList().MapTo<List<PermissionDto>>()

            };
        }
    }
}
