using System.Reflection;
using Abp.Application.Services;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;

namespace Fzrain
{
    [DependsOn(typeof(AbpWebApiModule), typeof(FzrainApplicationModule))]
    public class FzrainWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(FzrainApplicationModule).Assembly, "app")
                .Build();
        }
    }
}
