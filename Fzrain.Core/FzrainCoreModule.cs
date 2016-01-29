using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Fzrain.Authorization.Roles;
using Fzrain.Authorization.Users;

namespace Fzrain
{
    public class FzrainCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IRoleManagementConfig, RoleManagementConfig>();
            IocManager.Register<IUserManagementConfig, UserManagementConfig>();
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

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
