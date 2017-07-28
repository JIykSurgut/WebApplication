using System.Web;
using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/bootstrap-css").Include(
                      "~/Content/Bootstrap/css/bootstrap.min-{version}.css",
                      "~/Content/Bootstrap/css/bootstrap-theme-{version}.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-js").Include(
                      "~/Content/Bootstrap/js/bootstrap.min-{version}.js"));
        }
    }
}
