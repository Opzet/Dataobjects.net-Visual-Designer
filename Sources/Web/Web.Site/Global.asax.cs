using System.Web.Mvc;
using System.Web.Routing;
using TXSoftware.DataObjectsNetEntityModel.Web.Site.App_Data;
using RouteMagic;

namespace Web.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
//
//            routes.MapRoute(
//                "Default", // Route name
//                "{controller}/{action}/{id}", // URL with parameters
//                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
//            );

            //routes.MapPageRoute("versions", "latest-version/{currentversion}", "~/Versions.ashx");
            routes.MapHttpHandler<VersionsHandler>("versions", "versions/{mode}/{currentversion}/{isDebug}");//, "Versions.ashx");

            // to test service call:
            // http://localhost:85/ManagementService/productversions?withPrivate=true

            //RouteTable.Routes.Add(new ServiceRoute("ManagementService", new WebServiceHostFactory(), typeof(ManagementService)));
        }

        protected void Application_Start()
        {
            //  SessionManager.DomainBuilder = DomainBuilder.Build;

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}