using System.Web;
using System.Web.Optimization;

namespace SigesoftWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.12.1.js",
                        "~/Scripts/jquery.datetimepicker.full.js",
                        "~/Scripts/jquery.contextMenu.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/popper").Include(
                 "~/Scripts/umd/Popper.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
           "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                 "~/Scripts/fullcalendar.js",
                 "~/Scripts/locale/es.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      
                      "~/Scripts/bootstrap.js",

                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap3-typeahead.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                   "~/Scripts/main.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      
                      "~/Content/css/Estilos.css",
                       "~/Content/css/PatientsAssistance.css",
                        "~/Content/css/FloatLabel.css",
                      "~/Content/themes/base/jquery-ui.css",
                      "~/Content/css/jquery.datetimepicker.css",
                      "~/Content/jquery.contextMenu.css"
                      ));


        }
    }
}
