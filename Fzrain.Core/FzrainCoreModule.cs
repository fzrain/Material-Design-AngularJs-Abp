using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Fzrain.Authorization;


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
                  new XmlEmbeddedFileLocalizationDictionaryProvider(
                      Assembly.GetExecutingAssembly(),
                      "Fzrain.Localization.Source"
                      )
                  )
              );
            //  IocManager.Register<IPermissionChecker, PermissionChecker>();
            Configuration.Authorization.Providers.Add<FzrainAuthorizationProvider>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
