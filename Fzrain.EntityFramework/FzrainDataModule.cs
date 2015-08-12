using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using Fzrain.Authorization.Roles;
using Fzrain.Authorization.Users;
using Fzrain.EntityFramework;

namespace Fzrain
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(FzrainCoreModule))]
    public class FzrainDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IRoleManagementConfig, RoleManagementConfig>();
            IocManager.Register<IUserManagementConfig, UserManagementConfig>();
            //IocManager.Register<IRepository<User,long>, FzrainRepositoryBase<User,long>>();
            //IocManager.Register<IRepository<Role,int>,FzrainRepositoryBase<Role>>();
            //IocManager.Register<IRepository<PermissionSetting, long>, FzrainRepositoryBase<PermissionSetting, long>>();
            //IocManager.Register<IRepository<UserLogin, long>, FzrainRepositoryBase<UserLogin, long>>();
            //IocManager.Register<IRepository<Tenant,int>, FzrainRepositoryBase<Tenant>>();
            //IocManager.Register<IRepository<AuditLog, long>, FzrainRepositoryBase<AuditLog,long>>();
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<FzrainDbContext>(null);
        }
    }
}
