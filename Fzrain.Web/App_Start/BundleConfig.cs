using System.Web.Optimization;

namespace Fzrain.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            //VENDOR RESOURCES
            //~/Bundles/App/vendor/js
            bundles.Add(
                new ScriptBundle("~/Bundles/App/abp/js")
                    .Include(                      
                        "~/Abp/Framework/scripts/abp.js",
                        "~/Abp/Framework/scripts/libs/abp.jquery.js",
                        "~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/Abp/Framework/scripts/libs/abp.spin.js",
                        "~/Abp/Framework/scripts/libs/angularjs/abp.ng.js"
                    )
                );

            //~/Bundles/App/vendor/css
            //bundles.Add(
            //    new StyleBundle("~/Bundles/App/vendor/css")
            //        .Include(
            //            "~/Content/themes/base/all.css",
            //            "~/Content/bootstrap-cosmo.min.css",
            //            "~/Content/toastr.min.css",
            //            "~/Scripts/sweetalert/sweet-alert.css",
            //            "~/Content/flags/famfamfam-flags.css",
            //            "~/Content/font-awesome.min.css"
            //        )
            //    );

            //~/Bundles/App/vendor/js
            //bundles.Add(
            //    new ScriptBundle("~/Bundles/App/vendor/js")
            //        .Include(
            //            "~/Abp/Framework/scripts/utils/ie10fix.js",
            //            "~/Scripts/json2.min.js",

            //            "~/Scripts/modernizr-2.8.3.js",
                        
            //            "~/Scripts/jquery-2.1.3.min.js",
            //            "~/Scripts/jquery-ui-1.11.4.min.js",

            //            "~/Scripts/bootstrap.min.js",

            //            "~/Scripts/moment-with-locales.min.js",
            //            "~/Scripts/jquery.blockUI.js",
            //            "~/Scripts/toastr.min.js",
            //            "~/Scripts/sweetalert/sweet-alert.min.js",
            //            "~/Scripts/others/spinjs/spin.js",
            //            "~/Scripts/others/spinjs/jquery.spin.js",

            //            "~/Scripts/angular.min.js",
            //            "~/Scripts/angular-animate.min.js",
            //            "~/Scripts/angular-sanitize.min.js",
            //            "~/Scripts/angular-ui-router.min.js",
            //            "~/Scripts/angular-ui/ui-bootstrap.min.js",
            //            "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
            //            "~/Scripts/angular-ui/ui-utils.min.js",

            //            "~/Abp/Framework/scripts/abp.js",
            //            "~/Abp/Framework/scripts/libs/abp.jquery.js",
            //            "~/Abp/Framework/scripts/libs/abp.toastr.js",
            //            "~/Abp/Framework/scripts/libs/abp.blockUI.js",
            //            "~/Abp/Framework/scripts/libs/abp.spin.js",
            //            "~/Abp/Framework/scripts/libs/angularjs/abp.ng.js"
            //        )
            //    );

            //APPLICATION RESOURCES

            //~/Bundles/App/Main/css
           

            //~/Bundles/App/Main/js
         
        }
    }
}