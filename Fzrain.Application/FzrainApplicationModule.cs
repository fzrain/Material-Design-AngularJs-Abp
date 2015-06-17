using System.Reflection;
using Abp.Modules;

namespace Fzrain
{
    [DependsOn(typeof(FzrainCoreModule))]
    public class FzrainApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
