using System;
using System.Data;
using System.IO;
using System.Web.Mvc;
using Com.Centaline.Framework.Kernel.Json.Interface;
using Spring.Context.Attributes;
using Spring.Objects.Factory.Attributes;
using Spring.Objects.Factory.Support;
using SqlExport.Interface;
using SqlToExcel.Module.Common;
using System.Xml.Linq;

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
            return JsonSerializer.ToJson(result);
        }
   
        /// <summary>
        /// NPOI插件保存excel到网页，用于MVC4
        /// </summary>
        /// <returns></returns>
        public ActionResult DownLoadExcel()
        {
            string Path = AppDomain.CurrentDomain.BaseDirectory + "Resource\\Config\\DatetimeConfig.xml";
            XElement xele = XElement.Load(Path);
            if ((string)xele.Attribute("content") != ""&& (string)xele.Attribute("DateBegin")!=""&&(string)xele.Attribute("DateEnd") != "")
            {
                DateTime dateTime = DateTime.Parse((string) xele.Attribute("content"));
                DateTime dataTimeBegin= DateTime.Parse((string)xele.Attribute("DateBegin"));
                DateTime dataTimeEnd = DateTime.Parse((string)xele.Attribute("DateEnd"));
                TimeSpan span = DateTime.Now - dateTime;
                if (span.TotalMinutes < 30)
                {
                    //Response.Redirect("/Home/Index");
                    return this.Content("<script>alert('30分钟内不能重复导出到excel!')</script>");
                   
                }
                if (DateTime.Now < dataTimeBegin || DateTime.Now > dataTimeEnd)
                {
                    return this.Content("<script>alert('只能再指定的时间内导出到excel!')</script>");
                }
            }
            //延迟30分钟再可以提交
         
            string sql = "SELECT  [UserID],[UserCode],[UserName],[ParentID],[Position],[Mobile],[Email],[Levels],[AttentionTime]FROM Users WHERE CreateStatus = 1 AND(AttentionState = 1); ";
            DataTable dt = SqlExportService.GetDataTableFromSql(Request, sql);

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