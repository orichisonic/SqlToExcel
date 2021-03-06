﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SqlToExcel
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                   name: "Default",
                   url: "{controller}/{action}/{id}",
                   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
               );

            routes.MapRoute(
               name: "QueryPage",
               url: "QueryPage",
               defaults: new { controller = "SqlExportService", action = "QueryPage" }
           );

            routes.MapRoute(
              name: "ExportToCsv",
              url: "ExportToCsv",
              defaults: new { controller = "SqlExportService", action = "ExportToCsv" }
          );

            routes.MapRoute(
             name: "Login",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
         );
        }
    }
}
