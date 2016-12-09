using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Centaline.Framework.Kernel.Json.Interface;
using Com.Centaline.Framework.QuickQuery.Utils;
using Spring.Context.Attributes;
using Spring.Objects.Factory.Attributes;
using Spring.Objects.Factory.Support;
using Spring.Stereotype;
using SqlExport.Interface;

namespace SqlToExcel.Controllers
{

    [Spring.Stereotype.Controller]
    [Scope(ObjectScope.Prototype)]
    public class SqlExportServiceController : Controller
    {

        [Autowired]
        public ISqlExportService SqlExportService { get; set; }

        [Autowired]
        public IJsonSerializer JsonSerializer { get; set; }

        // GET: SqlExport
        public ActionResult Index()
        {
            return View();
        }


        public string QueryPage()
        {
            object obj = ObjectContainer.Instance.GetObject<ISqlExportService>().Query(Request);
            return ObjectContainer.Instance.GetObject<IJsonSerializer>().ToJson(obj);
           // return "";
        }

    }
}