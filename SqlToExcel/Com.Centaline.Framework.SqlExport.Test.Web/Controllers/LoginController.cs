using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Centaline.Framework.Kernel.Json.Interface;
using Spring.Context.Attributes;
using Spring.Objects.Factory.Attributes;
using Spring.Objects.Factory.Support;
using SqlToExcel.Module.Common;
using Com.Centaline.Framework.QuickQuery.Utils;

namespace SqlToExcel.Controllers
{
    [Spring.Stereotype.Controller]
    [Scope(ObjectScope.Prototype)]
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [Autowired]
        public IJsonSerializer JsonSerializer { get; set; }


        public string Submit()
         {

             string userName = Request["name"];
             string pwd = Request["pwd"];
            object result;
            if (UserSession.LoginAd(userName, pwd))
            {
                result= "验证通过";
            }
            else
            {
                result= "验证失败";
            }
            return ObjectContainer.Instance.GetObject<IJsonSerializer>().ToJson(result);
         }
    }
}