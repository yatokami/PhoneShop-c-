using System.Web.Optimization;

namespace PhoneShop
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Public/plugins/jQuery/jQuery-2.1.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootjs").Include(
                         "~/Public/bootstrap/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/appjs").Include(
                        "~/Public/dist/js/app.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/icheck").Include(
                 "~/Public/plugins/iCheck/icheck.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery/dataTables").Include(
                 "~/Public/plugins/datatables/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables/bootstrap").Include(
                 "~/Public/plugins/datatables/dataTables.bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery/slimscroll").Include(
                "~/Public/plugins/slimScroll/jquery.slimscroll.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/fastclick").Include(
                 "~/Public/plugins/fastclick/fastclick.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js.demo").Include(
                 "~/Public/dist/js/demo.js"));

            bundles.Add(new StyleBundle("~/bundles/bootcss").Include("~/Public/bootstrap/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/bundles/font-awesome").Include("~/Content/css/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/bundles/ionicons").Include("~/Content/css/ionicons.min.css"));

            bundles.Add(new StyleBundle("~/bundles/AdminLTE").Include("~/Public/dist/css/AdminLTE.min.css"));

            bundles.Add(new StyleBundle("~/bundles/skin-blue").Include("~/Public/dist/css/skins/skin-blue.min.css"));

            bundles.Add(new StyleBundle("~/bundles/blue").Include("~/Public/plugins/iCheck/square/blue.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
