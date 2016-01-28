using System.Reflection;
using Abp.Modules;
using Fzrain.Authorization.Roles;
using Fzrain.Authorization.Users;

namespace Fzrain
{
    public class FzrainCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IRoleManagementConfig, RoleManagementConfig>();
            IocManager.Register<IUserManagementConfig, UserManagementConfig>();
          //  IocManager.Register<IPermissionChecker, PermissionChecker>();

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
