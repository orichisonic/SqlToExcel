using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SqlExport.Interface;
using OfficeOpenXml;
using SqlExport.Config;

namespace SqlExport
{
    public  class DbToExcel : IDbToExcel
    {
        

        public void SaveExcel(string fileName, string sql, string sheetName)
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
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add(sheetName);
                    IDataReader reader = DBConfig.db.DBProvider.ExecuteReader(sql);
                    ws.Cells["A1"].LoadFromDataReader(reader, true);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                package.Save();
            }
        }

        public bool SaveExcel(ExcelPackage package, string sql, string SheetName)
        {

            try
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(SheetName);
                IDataReader reader = DBConfig.db.DBProvider.ExecuteReader(sql);
                ws.Cells["A1"].LoadFromDataReader(reader, true);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int ExportExcel(string FileName, string sql, string SheetName)
        {
            if (FileName != null)
            {
                Stopwatch watch = Stopwatch.StartNew();
                watch.Start();
                SaveExcel(FileName, sql, SheetName);
                watch.Stop();
                return Convert.ToInt32(watch.ElapsedMilliseconds/1000);
            }

            return -1;
        }

        public async Task<int> ExportExcelAsync(string FileName, string sql, string SheetName)
        {
            return await Task.Run(
                () => { return ExportExcel(FileName, sql, SheetName); }
                );
        }

        /// <summary>
        /// 多表格导出到一个EXCEL工作簿
        /// </summary>
        public int ExportExcel(string[] TabelNameArray, string filename)
        {
            if (filename != null)
            {
                Stopwatch watch = Stopwatch.StartNew();
                watch.Start();

                FileInfo newFile = new FileInfo(filename);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(filename);
                }


                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    for (int i = 0; i < TabelNameArray.Length; i++)
                    {
                        string sql = "select * from  [" + TabelNameArray[i] + "]";
                        IDataReader reader = DBConfig.db.DBProvider.ExecuteReader(sql);
                        SaveExcel(package, sql, TabelNameArray[i]);

                    }
                    package.Save();
                }
                watch.Stop();
                return Convert.ToInt32(watch.ElapsedMilliseconds/1000);
            }

            return -1;
        }

        public async Task<int> ExportExcelAsync(string[] TabelNameArray, string filename)
        {
            return await Task.Run(
                () => { return ExportExcel(TabelNameArray, filename); }
                );
        }

        public int ExportExcel(string[] TabelNameArray, string filename, string[] whereSQLArr)
        {
            if (filename != null)
            {
                Stopwatch watch = Stopwatch.StartNew();
                watch.Start();
                FileInfo file = new FileInfo(filename);
                int wherecount = whereSQLArr.Length;
                int count = TabelNameArray.Length;
                for (int i = 0; i < wherecount; i++)
                {
                    string s = filename.Substring(0, filename.LastIndexOf("\\"));
                    StringBuilder newfileName = new StringBuilder(s);
                    int index = whereSQLArr[i].LastIndexOf("=");
                    string sp = whereSQLArr[i].Substring(index + 2, whereSQLArr[i].Length - index - 3);
                    newfileName.Append("\\" + file.Name.Substring(0, file.Name.LastIndexOf(".")) + "_" + sp + ".xlsx");
                    FileInfo newFile = new FileInfo(newfileName.ToString());
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(newfileName.ToString());
                    }

                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        for (int j = 0; j < count; j++)
                        {
                            string sql = "select * from [" + TabelNameArray[j] + "]" + whereSQLArr[i];
                            IDataReader reader = DBConfig.db.DBProvider.ExecuteReader(sql);
                            SaveExcel(package, sql, TabelNameArray[j]);

                        }

                        package.Save();
                    }



                }

                watch.Stop();
                return Convert.ToInt32(watch.ElapsedMilliseconds/1000);
            }

            return -1;
        }

        public async Task<int> ExportExcelAsync(string[] TabelNameArray, string filename, string[] whereSQLArr)
        {
            return await Task.Run(
                () => { return ExportExcel(TabelNameArray, filename, whereSQLArr); }
                );

        }
    }
}
