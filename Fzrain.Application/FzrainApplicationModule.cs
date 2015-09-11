using System.Reflection;
using Abp.Modules;
using Fzrain.Authorization;

namespace Fzrain
{
    [DependsOn(typeof(FzrainCoreModule))]
    public class FzrainApplicationModule : AbpModule
    {
     
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Configuration.Authorization.Providers.Add<FzrainAuthorizationProvider>();
        }
    }
}
