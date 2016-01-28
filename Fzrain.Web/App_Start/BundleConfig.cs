using System.Web.Optimization;

namespace Fzrain.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

          
            //~/Bundles/App/vendor/js
            bundles.Add(
                new ScriptBundle("~/Bundles/App/abp/js")
                    .Include(
                        "~/bower_components/abp-resources/Abp/Framework/scripts/abp.js",
                        "~/bower_components/abp-resources/Abp/Framework/scripts/libs/abp.jquery.js",
                        //"~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/bower_components/abp-resources/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/bower_components/abp-resources/Abp/Framework/scripts/libs/abp.spin.js",
                        "~/js/abp.ng.js"
                    )
                );
            //~/Bundles/App/Main/js
            bundles.Add(
                new ScriptBundle("~/Bundles/Views/js")
                    .IncludeDirectory("~/Views", "*.js", true)
                );
         
        }
    }
}