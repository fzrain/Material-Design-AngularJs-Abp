using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using Fzrain.Authorization;
using Fzrain.Authorization.Roles;


namespace Fzrain
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class FzrainCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.MultiTenancy.IsEnabled = true;
            Configuration.Localization.Sources.Add(
              new DictionaryBasedLocalizationSource(
                  FzrainConsts.LocalizationSourceName,
                  new JsonEmbeddedFileLocalizationDictionaryProvider(
                      Assembly.GetExecutingAssembly(),
                      "Fzrain.Localization.Source"
                      )
                  )
              );
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);
            //  IocManager.Register<IPermissionChecker, PermissionChecker>();
            Configuration.Authorization.Providers.Add<FzrainAuthorizationProvider>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
