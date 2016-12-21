using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime;
using Com.Centaline.Framework.Kernel.Json.Interface;
using Com.Centaline.Framework.QuickQuery.Utils;
using Spring.Context.Attributes;
using Spring.Objects.Factory.Attributes;
using Spring.Objects.Factory.Support;
using Spring.Stereotype;
using SqlExport.Interface;
using SqlToExcel.Module.Common;

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
        }

        public string ExportToCsv()
        {
            object obj = ObjectContainer.Instance.GetObject<ISqlExportService>().ExportToCsv(Request);
            return ObjectContainer.Instance.GetObject<IJsonSerializer>().ToJson(obj);
            // return "";
        }

        public string IsLogin()
        {
               object result;
            if (UserSession.IsLogin() == true)
            {
                result = "登录成功";
            }
            else
            {
                result = "登录失败";
            }
            return ObjectContainer.Instance.GetObject<IJsonSerializer>().ToJson(result);
        }

        public string ExportToExcel()
        {
            object result;
            var userLoginsession = System.Web.HttpContext.Current.Session[ConstValue.UserLoginSesstionKeyName];
            if (userLoginsession ==null)
            {
                result= "用户没有登录";
            }
            foreach (string whitelist in ConfigInfo.WhiteList.Split(','))
            {
                if ((userLoginsession as UserLoginInfo).UserName == whitelist)
                {
                    string sql = "SELECT  [UserID],[UserCode],[UserName],[ParentID],[Position],[Mobile],[Email],[Levels],[AttentionTime]FROM Users WHERE CreateStatus = 1 AND(AttentionState = 1); ";
                    object obj = ObjectContainer.Instance.GetObject<ISqlExportService>().ExportToExcel(Request,sql);
                    return ObjectContainer.Instance.GetObject<IJsonSerializer>().ToJson(obj);
                }
            }

            result = "用户没有权限执行导出功能";
            return ObjectContainer.Instance.GetObject<IJsonSerializer>().ToJson(result); 
        }
    }
}