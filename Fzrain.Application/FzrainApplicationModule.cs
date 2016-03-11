using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;

namespace Fzrain
{
    [DependsOn(typeof(FzrainCoreModule), typeof(AbpAutoMapperModule))]
    public class FzrainApplicationModule : AbpModule
    {
     
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
           
        }
    }
}
