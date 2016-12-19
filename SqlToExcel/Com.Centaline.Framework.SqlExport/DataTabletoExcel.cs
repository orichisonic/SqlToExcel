using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using OfficeOpenXml;
using System.Diagnostics;
using SqlExport.Config;
using SqlExport.Interface;


namespace SqlExport
{
    public class DataTableToExcel: IDataTabletoExcel
    {

        public  DataTable GetDataTableFromSql(string sql)
        {
            DataTable dt = new DataTable();
            dt = DbConfig.Db.DbProvider.ReturnDataTable(sql);
            return dt;
        }

        public  DataTable GetDataTableFromName(string tableName)
        {
            DataTable dt = new DataTable();
            string sql = "select * from  [" + tableName + "]";
            dt = DbConfig.Db.DbProvider.ReturnDataTable(sql);
            return dt;
        }

        /// <summary>
        /// 已有工作簿中，添加新的sheet并保存
        /// </summary>
        public  bool SaveExcel(string sheetName, DataTable dt, ExcelPackage package)
        {
            try
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(sheetName);
                ws.Cells["A1"].LoadFromDataTable(dt, true);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存excel文件，覆盖相同文件名的文件
        /// </summary>
        public  void SaveExcel(string fileName, DataTable dt, string newSheetName)
        {
            FileInfo newFile = new FileInfo(fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileName);
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                try
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add(newSheetName);
                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                package.Save();
            }
        }

        /// <summary>
        /// 单表格导出到excel工作簿
        /// </summary>
        public  int ExportTemplate(string tabelName, string filename)
        {
            if (filename != null)
            {
                Stopwatch watch = Stopwatch.StartNew();
                watch.Start();
                string sql = "select * from  [" + tabelName + "]" + " where 1=2";
                DataTable dt = GetDataTableFromSql(sql);
                SaveExcel(filename, dt, tabelName);
                watch.Stop(); 
                return Convert.ToInt32(watch.ElapsedMilliseconds / 1000);
            }
            return -1;
        }

        /// <summary>
        /// 单表格导出到一个excel工作簿
        /// </summary>
        public int ExportExcel(string sheetName, string sql, string filename)
        {     
            if (filename != null)
            {
                Stopwatch watch = Stopwatch.StartNew();
                watch.Start();
                DataTable dt = GetDataTableFromSql(sql);
                SaveExcel(filename, dt, sheetName);
                watch.Stop();
                return Convert.ToInt32(watch.ElapsedMilliseconds / 1000);
            }
            return -1;
        }

        /// <summary>
        /// 单表格导出到多excel工作簿，分页版本
        /// </summary>
        public int ExportExcel(string tabelName, int pageSize, string filename)
        {
            if (filename != null)
            {
                Stopwatch watch = Stopwatch.StartNew();
                watch.Start();
                int recordCount = DbConfig.Db.DbProvider.ReturnTbCount(tabelName);
                string sql = "select * from  [" + tabelName + "]";
                int workBookCount = (recordCount - 1) / pageSize + 1;
                FileInfo newFile = new FileInfo(filename);
                for (int i = 1; i <= workBookCount; i++)
                {
                    string s = filename.Substring(0, filename.LastIndexOf("."));
                    StringBuilder newfileName = new StringBuilder(s);
                    newfileName.Append(i + ".xlsx");
                    newFile = new FileInfo(newfileName.ToString());
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(newfileName.ToString());
                    }
                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        DataTable dt = DbConfig.Db.DbProvider.ReturnDataTable(sql, pageSize * (i - 1), pageSize);
                        SaveExcel(tabelName, dt, package);
                        package.Save();
                    }
                }
                watch.Stop();
                return Convert.ToInt32(watch.ElapsedMilliseconds / 1000);
            }
            return -1;
        }

        /// <summary>
        /// 多表格导出到一个excel工作簿
        /// </summary>
        public int ExportExcel(IList<string> tabelNames, string filename)
        {
            if (filename != null)
            {
                Stopwatch watch = Stopwatch.StartNew();
                watch.Start();
                IList<string> sqls = new List<string>();
                IList<string> sheetNames = new List<string>();
                foreach (var item in tabelNames)
                {
                    sheetNames.Add(item.ToString());
                    sqls.Add("select * from  [" + item.ToString() + "]");
                }
                DataTable dt = new DataTable();

                FileInfo newFile = new FileInfo(filename);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(filename);
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    for (int i = 0; i < sqls.Count; i++)
                    {
                        dt = DbConfig.Db.DbProvider.ReturnDataTable(sqls[i]);
                        SaveExcel(sheetNames[i], dt, package);

                    }
                    package.Save();
                }
                watch.Stop();
                return Convert.ToInt32(watch.ElapsedMilliseconds / 1000);
            }

            return -1;
        }
    }
}
