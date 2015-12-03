using System.Web.Optimization;

namespace StockSharer.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/global").Include(
                "~/assets/js/lib/jquery-1.11.3.min.js",
                "~/assets/js/lib/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/bundles/styles/global").Include(
                "~/assets/css/lib/bootstrap.min.css",
                "~/assets/css/global/style.css"));

            bundles.Add(new StyleBundle("~/bundles/styles/search").Include(
                "~/assets/css/search/results.css"));

            bundles.Add(new StyleBundle("~/bundles/styles/user").Include(
                "~/assets/css/user/login.css"));
        }
    }
}