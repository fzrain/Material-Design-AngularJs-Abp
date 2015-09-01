using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fzrain.Authorization.Roles;
using Fzrain.Roles.Dto;

namespace Fzrain.Roles
{
   public  class RoleAppService:ApplicationService,IRoleAppService
   {
       private readonly RoleManager roleManager;
       private readonly IRepository<Role> roleRepository;

       public RoleAppService(RoleManager roleManager, IRepository<Role> roleRepository)
       {
           this.roleManager = roleManager;
           this.roleRepository = roleRepository;
       }

       public PagedResultOutput<RoleDto> GetRoles(RoleQueryInput input)
       {
             return new PagedResultOutput<RoleDto>
                   {
                       Items = roleManager.Roles.OrderByDescending(r=>r.CreationTime).PageBy(input).ToList().MapTo<List<RoleDto>>(),
                       TotalCount =roleManager.Roles.Count()
                   };          
       }

       public dynamic GetAllRoleList()
       {
           return roleManager.Roles.MapTo<List<RoleDto>>().Select(r=>new {r.Id,r.Name,r.IsDefault});
       }

       public async  Task AddOrUpdate(RoleDto roleDto)
       {
           Role role= roleDto.MapTo<Role>();
           role.DisplayName = role.Name;
           role.TenantId = AbpSession.TenantId;
           await  roleRepository.InsertOrUpdateAsync(role);
       }

       public async  Task<RoleDto> GetById(IdInput input)
       {
           var role = await  roleManager.GetRoleByIdAsync(input.Id);
           return role.MapTo<RoleDto>();
       }

       public async  Task Delete(IdInput input)
       {
          await  roleRepository.DeleteAsync(input.Id);
       }
   }
}   
