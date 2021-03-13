using System.Web;
using System.Web.Optimization;

namespace TExp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-2.1.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/angular-toastr.min.css",
                "~/Content/loading-bar.min.css",
                "~/Content/site.css",
                "~/Content/step-wizard.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js", "~/Scripts/angular-messages.*", "~/Scripts/angular-animate.*", "~/Scripts/i18n/angular-locale_ru.js",
                "~/Scripts/angular-ui/ui-bootstrap.*", "~/Scripts/angular-ui/ui-bootstrap-tpls.*",
                "~/Scripts/check-list.js",
                "~/Scripts/angular-toastr.min.js", "~/Scripts/angular-toastr.tpls.min.js",
                "~/Scripts/loading-bar.min.js",
                "~/Scripts/app/texp-main.*",
               // "~/Scripts/app/texp-index.*",
                "~/Scripts/app/texp-equipment.*",
                "~/Scripts/app/texp-wizard.*",
                "~/Scripts/app/texp-decomission.*",
                "~/Scripts/app/texp-list.*",
                "~/Scripts/app/texp-user-edit.*",
                "~/Scripts/app/texp-reset-password.*",
                "~/Scripts/app/texp-expertise-list.*"
                //"~/Scripts/angular-sanitize.js", "~/Scripts/angular-animate.js","~/Scripts/angular-strap.js", "~/Scripts/angular-strap.tpl.js","~/Scripts/parse-options.js"
                )); //

            bundles.Add(new ScriptBundle("~/bundles/flow").Include(
           "~/Scripts/flow/flow.*", "~/Scripts/ng-flow/ng-flow.*"));

            bundles.Add(new ScriptBundle("~/bundles/server-validate").Include(
          "~/Scripts/app/ng-server-validate.*"));



            bundles.Add(new ScriptBundle("~/bundles/texp-index").Include(
                "~/Scripts/app/texp-index.*"
                ));

            bundles.Add(new ScriptBundle("~/bundles/texp-malfunction").Include(
               "~/Scripts/app/texp-malfunction.*"
               ));

            bundles.Add(new ScriptBundle("~/bundles/texp-type-equipment").Include(
                "~/Scripts/app/texp-type-equipment.*"
                ));

            bundles.Add(new ScriptBundle("~/bundles/texp-message").Include(
               "~/Scripts/app/texp-message.*"
               ));

            bundles.Add(new ScriptBundle("~/bundles/texp-message-two").Include(
              "~/Scripts/app/texp-message-two.*"
              ));

            bundles.Add(new ScriptBundle("~/bundles/texp-user-list").Include(
              "~/Scripts/app/texp-user-list.*"
              ));

            bundles.Add(new ScriptBundle("~/bundles/texp-document-list").Include(
             "~/Scripts/app/texp-document-list.*"
             ));

        }
    }
}
