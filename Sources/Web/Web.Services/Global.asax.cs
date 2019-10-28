using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using TXSoftware.DataObjectsNetEntityModel.Web.Model;
using Xtensive.Practices.Web;

namespace TXSoftware.DataObjectsNetEntityModel.Web.Services
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            SessionManager.DomainBuilder = DomainBuilder.Build;
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
//            // Edit the base address of DOEMDService by replacing the "DOEMDService" string below
//            RouteTable.Routes.Add(new ServiceRoute("DOEMDService", new WebServiceHostFactory(),
//                typeof(ManagementService)));
        }
    }
}
