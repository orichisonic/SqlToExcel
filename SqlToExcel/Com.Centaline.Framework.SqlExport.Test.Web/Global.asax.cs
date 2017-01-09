using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SqlToExcel.Module.ClassLibrary;

namespace SqlToExcel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            new ServiceLocatorFatory().Create();
            AreaRegistration.RegisterAllAreas();
         
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new SpringControllerFactory());

        }
    }
}
