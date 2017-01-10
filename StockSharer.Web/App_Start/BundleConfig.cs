using System.Web.Optimization;

namespace StockSharer.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/global").Include(
                "~/assets/js/lib/jquery-1.11.3.min.js",
                "~/assets/js/lib/bootstrap.min.js",
                "~/assets/js/lib/validator.js",
                "~/assets/js/lib/cookies.js",
                "~/assets/js/lib/handlebars-v4.0.5.js"));

            bundles.Add(new StyleBundle("~/bundles/styles/global").Include(
                "~/assets/css/lib/bootstrap.min.css",
                "~/assets/css/global/style.css"));

            bundles.Add(new StyleBundle("~/bundles/styles/search").Include(
                "~/assets/css/search/results.css"));

            bundles.Add(new StyleBundle("~/bundles/styles/user").Include(
                "~/assets/css/user/login.css"));

            bundles.Add(new StyleBundle("~/bundles/styles/settings").Include(
                "~/assets/css/settings/settings.css",
                "~/assets/css/lib/live-search.css"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/search").Include(
                "~/assets/js/search/reserve.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/requests").Include(
                "~/assets/js/settings/requests.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/myGames").Include(
                "~/assets/js/lib/foot-2-simple-ajax.js",
                "~/assets/js/lib/foot-2-live-search.js",
                "~/assets/js/settings/addGame.js"));

            bundles.Add(new StyleBundle("~/bundles/styles/settings/userAvailability").Include(
                "~/assets/css/lib/bootstrap-timepicker.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/userAvailability").Include(
                "~/assets/js/lib/bootstrap-timepicker.min.js",
                "~/assets/js/settings/userAvailability.js"));
        }
    }
}