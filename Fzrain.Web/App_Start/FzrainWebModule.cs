using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Localization.Sources.Xml;
using Abp.Modules;

namespace Fzrain.Web
{
    [DependsOn(typeof(FzrainDataModule), typeof(FzrainApplicationModule), typeof(FzrainWebApiModule))]
    public class FzrainWebModule : AbpModule
    {
        public override void PreInitialize()
        {
           
            //Add/remove languages for your application
            Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england"));
           // Configuration.Localization.Languages.Add(new LanguageInfo("tr", "Türkçe", "famfamfam-flag-tr"));
            Configuration.Localization.Languages.Add(new LanguageInfo("zh-cn", "简体中文", "famfamfam-flag-cn", true));
            //Add/remove localization sources here          
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    FzrainConsts.LocalizationSourceName,
                new XmlFileLocalizationDictionaryProvider(
                    HttpContext.Current.Server.MapPath("~/Localization/Fzrain")
                    )));
            //Configure navigation/menu
           // Configuration.Navigation.Providers.Add<FzrainNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
