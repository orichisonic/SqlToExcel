using System;
using System.Data;
using System.IO;
using System.Web.Mvc;
using Com.Centaline.Framework.Kernel.Json.Interface;
using Com.Centaline.Framework.QuickQuery.Utils;
using Spring.Context.Attributes;
using Spring.Objects.Factory.Attributes;
using Spring.Objects.Factory.Support;
using SqlExport.Interface;
using SqlToExcel.Module.Common;
using System.Xml.Linq;
using SqlToExcel.Module.Mail;

namespace SqlToExcel.Controllers
{

    [Spring.Stereotype.Controller]
    [Scope(ObjectScope.Prototype)]
    public class SqlExportServiceController : Controller
    {

        public static DateTime DateTime ;
        [Autowired]
        public ISqlExportService SqlExportService { get; set; }

        [Autowired]
        public IJsonSerializer JsonSerializer { get; set; }

        // GET: SqlExport
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 输出数据到二维表
        /// </summary>
        /// <returns></returns>
        public string QueryPage()
        {
            object obj = ObjectContainer.Instance.GetObject<ISqlExportService>().Query(Request);
            return ObjectContainer.Instance.GetObject<IJsonSerializer>().ToJson(obj);
        }

        /// <summary>
        /// openxml导出csv到服务器本地
        /// </summary>
        /// <returns></returns>
        public string ExportToCsv()
        {
            object obj = ObjectContainer.Instance.GetObject<ISqlExportService>().ExportToCsv(Request);
            return ObjectContainer.Instance.GetObject<IJsonSerializer>().ToJson(obj);
            // return "";
        }

        /// <summary>
        /// 判断是否有session
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// openxml导出excel到服务器本地
        /// </summary>
        /// <returns></returns>
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
                    object obj = ObjectContainer.Instance.GetObject<ISqlExportService>().ExportToExcel(Request, sql);
                    return ObjectContainer.Instance.GetObject<IJsonSerializer>().ToJson(obj);
                    //ExportExcelForPercentForWeb("test", "a.xlsx");
                    //outExcel("test", "e:\\a.xlsx");
                    result = "导出excel成功";
                    return ObjectContainer.Instance.GetObject<IJsonSerializer>().ToJson(result);
                }
            }

            result = "用户没有权限执行导出功能";
            return ObjectContainer.Instance.GetObject<IJsonSerializer>().ToJson(result); 
        }
      

        public string SendMail()
        {
            Sendmail.sendMail("testTopic","d:\a.xls","testBody");
            return "";

        }

        /// <summary>
        /// NPOI插件保存excel到网页，用于MVC4
        /// </summary>
        /// <returns></returns>
        public ActionResult DownLoadExcel()
        {
            string Path = AppDomain.CurrentDomain.BaseDirectory + "Resource\\Config\\DatetimeConfig.xml";
            XElement xele = XElement.Load(Path);
            if ((string)xele.Attribute("content") != "")
            {
                DateTime dateTime = DateTime.Parse((string) xele.Attribute("content"));
                TimeSpan span = DateTime.Now - dateTime;
                if (span.TotalMinutes < 30)
                {
                    //Response.Redirect("/Home/Index");
                    return this.Content("<script>alert('30分钟内不能重复导出到excel!')</script>");
                   
                    return this.Content("alert('作成功')", "application/x-javascript");
                    return Content("alert('购物订单成功处理！');", "text/javascript");
                    //string script = string.Format("alert('库存不足! ({0})');", ":");
                    //return JavaScript(script);
                    return JavaScript("alert('30分钟内不能重复导出到excel!');");
                    //return Content("<font color='red'>你好啊ContentResult</font>");
                    // return new EmptyResult(); 

                }
            }
            //延迟30分钟再可以提交
         
            string sql = "SELECT  [UserID],[UserCode],[UserName],[ParentID],[Position],[Mobile],[Email],[Levels],[AttentionTime]FROM Users WHERE CreateStatus = 1 AND(AttentionState = 1); ";
            DataTable dt = ObjectContainer.Instance.GetObject<ISqlExportService>().GetDataTableFromSql(Request, sql);

            //创建Excel文件的对象  
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet  
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题  
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            //row1.RowStyle.FillBackgroundColor = "";  
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                row1.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }
            //将数据逐步写入sheet1各个行  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString().Trim());
                }
            }
            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间  
            // 写入到客户端   
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            DateTime = DateTime.Now;
            xele.SetAttributeValue("content", DateTime);
            xele.Save(AppDomain.CurrentDomain.BaseDirectory + "Resource\\Config\\DatetimeConfig.xml");
            return File(ms, "application/vnd.ms-excel", strdate + "Excel.xls");
        }

     
    }
}