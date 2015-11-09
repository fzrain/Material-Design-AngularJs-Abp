using System.Collections.Generic;
using System.Linq;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Localization;
using Fzrain.Authorization.Permissions;

namespace Fzrain.Authorization
{
    public class FzrainAuthorizationProvider : AuthorizationProvider
    {
        private readonly IRepository<PermissionInfo> permissionRepository;

        public FzrainAuthorizationProvider(IRepository<PermissionInfo> permissionRepository)
        {
            this.permissionRepository = permissionRepository;
        }

        private  IEnumerable<PermissionInfo> PermissionInfos=> permissionRepository.GetAllList();
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var rootPermission = PermissionInfos.Where(p => p.ParentName == "无").First();
            var permission = context.CreatePermission(rootPermission.Name, new FixedLocalizableString(rootPermission.DisplayName), rootPermission.IsGrantedByDefault);
            AddPermissions(rootPermission, permission);
        }
        /// <summary>
        /// 递归加载所有权限
        /// </summary>
        /// <param name="rootPermission"></param>
        /// <param name="permission"></param>
        private void AddPermissions(PermissionInfo rootPermission, Permission permission)
        {
            var permissions = PermissionInfos.Where(p => p.ParentName == rootPermission.Name);
            foreach (PermissionInfo permissionInfo in permissions)
            {
                var childernPermission = permission.CreateChildPermission(permissionInfo.Name,
                      new FixedLocalizableString(permissionInfo.DisplayName), permissionInfo.IsGrantedByDefault);
                AddPermissions(permissionInfo, childernPermission);
            }
        }
    }
}
