using System.Web.Mvc;
using System.Web.Routing;
using FinanceManager.Domain.Concrete;
using System.Data.Entity;

namespace FinanceManager.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Database.SetInitializer<EFDbContext>(null);
        }
    }
}
