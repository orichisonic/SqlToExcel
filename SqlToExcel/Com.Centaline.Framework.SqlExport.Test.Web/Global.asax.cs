using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SqlExport;
using SqlToExcel.Controllers;
using SqlToExcel.Module.ClassLibrary;

namespace SqlToExcel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           SqlExportContext.Init(new ServiceLocatorFatory().Create());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new SpringControllerFactory());

        }
    }
}
