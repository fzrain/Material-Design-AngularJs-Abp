using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using Fzrain.EntityFramework;

namespace Fzrain
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(FzrainCoreModule))]
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
