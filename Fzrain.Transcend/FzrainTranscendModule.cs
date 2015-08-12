using System.Reflection;
using Abp.Modules;

namespace Fzrain.Transcend
{
   public  class FzrainTranscendModule:AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
