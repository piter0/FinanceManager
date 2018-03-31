using System.Web.Mvc;
using System.Web.Routing;

namespace FinanceManager.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Sorting", "{controller}/{date}/Sort-by-{sortBy}/{page}",
                new { action = "Index", page = UrlParameter.Optional} );

            routes.MapRoute("NavigationMenu", "{controller}/Select-date-{date}",
                new { controller = "Expense", action = "Index" });

            routes.MapRoute("Category", "ManageCategory/{type}",
                new { controller = "Category", action = "Index", });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Expense", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
