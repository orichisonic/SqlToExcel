using System.Web.Mvc;
using Spring.Context.Attributes;
using Spring.Objects.Factory.Attributes;
using Spring.Objects.Factory.Support;

namespace SqlToExcel.Controllers
{
    [Spring.Stereotype.Controller]
    [Scope(ObjectScope.Prototype)]
    public class HomeController : Controller
    {
       

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}