using System;
using System.Web.Mvc;
using System.Web.Routing;
using SqlToExcel.Controllers;

namespace SqlToExcel.Module.ClassLibrary
{
    public class SpringControllerFactory : DefaultControllerFactory
    {
        public SpringControllerFactory(string containerName = "")
        {

        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (null == controllerType)
            {
                requestContext.HttpContext.Response.StatusCode = 404;
                requestContext.RouteData.Values["controller"] = "error";
                requestContext.RouteData.Values["action"] = "error404";
                return new ErrorController();
            }

            IController controller = (IController)ServiceLocatorFatory.ObjectContainer.GetService(controllerType);
            return controller;
        }
    }
}