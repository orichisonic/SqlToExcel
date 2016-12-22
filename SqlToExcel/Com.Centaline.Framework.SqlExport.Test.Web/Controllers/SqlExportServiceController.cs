using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Antlr.Runtime;
using Com.Centaline.Framework.Kernel.Json.Interface;
using Com.Centaline.Framework.QuickQuery.Utils;
using Spring.Context.Attributes;
using Spring.Objects.Factory.Attributes;
using Spring.Objects.Factory.Support;
using Spring.Stereotype;
using SqlExport.Interface;
using SqlToExcel.Module.Common;
using System.Xml;
using org.in2bits.MyXls;

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

        /// <summary>
        /// NPOI插件保存excel到网页，用于MVC4
        /// </summary>
        /// <returns></returns>
        public FileResult DownLoadExcel()
        {
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
            return File(ms, "application/vnd.ms-excel", strdate + "Excel.xls");
        }

        /// <summary>
        /// MyXls组件保存网页Excel，用于webform的Page.Response
        /// </summary>
        public void ExportExcelForPercentForWeb(string sheetName, string xlsname)
        {
            XlsDocument xls = new XlsDocument();
            Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);
            try
            {
                string sql = "SELECT  [UserID],[UserCode],[UserName],[ParentID],[Position],[Mobile],[Email],[Levels],[AttentionTime]FROM Users WHERE CreateStatus = 1 AND(AttentionState = 1); ";
                DataTable table = ObjectContainer.Instance.GetObject<ISqlExportService>().GetDataTableFromSql(Request, sql);


                if (table == null || table.Rows.Count == 0) { return; }
                //XlsDocument xls = new XlsDocument();
                //Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);

                //填充表头  
                foreach (DataColumn col in table.Columns)
                {
                    sheet.Cells.Add(1, col.Ordinal + 1, col.ColumnName);
                }

                //填充内容  
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        sheet.Cells.Add(i + 2, j + 1, table.Rows[i][j].ToString());
                    }
                }

                //保存  
                //xls.FileName = xlsname;
                //xls.Save();

                #region 客户端保存
                using (MemoryStream ms = new MemoryStream())
                {
                    xls.Save(ms);
                    ms.Flush();
                    ms.Position = 0;
                    sheet = null;
                    xls = null;
                    HttpResponse response = System.Web.HttpContext.Current.Response;
                    response.Clear();

                    response.Charset = "UTF-8";
                    response.ContentType = "application/vnd.ms-excel";//"application/vnd.ms-excel";
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + xlsname));
                    //System.Web.HttpContext.Current.Response.WriteFile(fi.FullName);
                    byte[] data = ms.ToArray();
                    System.Web.HttpContext.Current.Response.BinaryWrite(data);
                   
                }

                #endregion
                //xls = null;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                sheet = null;
                xls = null;
            }

        }

        /// <summary>
        /// MyXls组件保存本地Excel，用于webform的Page.Response
        /// </summary>
        public  void ExportExcelForPercent(string sheetName, string xlsname)
        {
            string sql = "SELECT  [UserID],[UserCode],[UserName],[ParentID],[Position],[Mobile],[Email],[Levels],[AttentionTime]FROM Users WHERE CreateStatus = 1 AND(AttentionState = 1); ";
            DataTable table = ObjectContainer.Instance.GetObject<ISqlExportService>().GetDataTableFromSql(Request, sql);
          

            if (table == null || table.Rows.Count == 0) { return; }
            XlsDocument xls = new XlsDocument();
            Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);

            //填充表头  
            foreach (DataColumn col in table.Columns)
            {
                sheet.Cells.Add(1, col.Ordinal + 1, col.ColumnName);
            }

            //填充内容  
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    sheet.Cells.Add(i + 2, j + 1, table.Rows[i][j].ToString());
                }
            }

            //保存  
            xls.FileName = xlsname;
            xls.Save();
            xls = null;
        }

    }
}