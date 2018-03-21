using System.Web.Mvc;
using System.Web.Routing;

namespace FinanceManager.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("ExpenseActions", "{controller}/{action}/{expenseID}",
                new { controller = "Expense"});

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Expense", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
