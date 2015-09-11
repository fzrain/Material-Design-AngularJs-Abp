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
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<FzrainDbContext>(null);
        }
    }
}
