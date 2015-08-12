using System.Reflection;
using Abp.Modules;

namespace Fzrain
{
    public class FzrainCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
          


        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
