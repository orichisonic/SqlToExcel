using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spring.Context.Attributes;
using Spring.Objects.Factory.Support;

namespace SqlToExcel.Controllers
{

    [Spring.Stereotype.Controller]
    [Scope(ObjectScope.Prototype)]
    public class LoginDatabaseController : Controller
    {
        // GET: LoginDatabase
        public ActionResult LoginDatabase()
        {
            return View();
        }
    }
}