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
        

        public int SaveExcel(string fileName, string sql, string sheetName)
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
                    IDataReader reader = DbConfig.Db.DbProvider.ExecuteReader(sql);
                    ws.Cells["A1"].LoadFromDataReader(reader, true);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                package.Save();
                return 1;
            }
        }

        public bool SaveExcel(ExcelPackage package, string sql, string sheetName)
        {

            try
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(sheetName);
                IDataReader reader = DbConfig.Db.DbProvider.ExecuteReader(sql);
                ws.Cells["A1"].LoadFromDataReader(reader, true);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int ExportExcel(string fileName, string sql, string sheetName)
        {
            if (fileName != null)
            {
                Stopwatch watch = Stopwatch.StartNew();
                watch.Start();
                SaveExcel(fileName, sql, sheetName);
                watch.Stop();
                return Convert.ToInt32(watch.ElapsedMilliseconds/1000);
            }

            return -1;
        }

        public async Task<int> ExportExcelAsync(string fileName, string sql, string sheetName)
        {
            return await Task.Run(
                () => { return ExportExcel(fileName, sql, sheetName); }
                );
        }

        /// <summary>
        /// 多表格导出到一个EXCEL工作簿
        /// </summary>
        public int ExportExcel(string[] tabelNameArray, string filename)
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
                    for (int i = 0; i < tabelNameArray.Length; i++)
                    {
                        string sql = "select * from  [" + tabelNameArray[i] + "]";
                        IDataReader reader = DbConfig.Db.DbProvider.ExecuteReader(sql);
                        SaveExcel(package, sql, tabelNameArray[i]);

                    }
                    package.Save();
                }
                watch.Stop();
                return Convert.ToInt32(watch.ElapsedMilliseconds/1000);
            }

            return -1;
        }

        public async Task<int> ExportExcelAsync(string[] tabelNameArray, string filename)
        {
            return await Task.Run(
                () => { return ExportExcel(tabelNameArray, filename); }
                );
        }

        public int ExportExcel(string[] tabelNameArray, string filename, string[] whereSqlArr)
        {
            if (filename != null)
            {
                Stopwatch watch = Stopwatch.StartNew();
                watch.Start();
                FileInfo file = new FileInfo(filename);
                int wherecount = whereSqlArr.Length;
                int count = tabelNameArray.Length;
                for (int i = 0; i < wherecount; i++)
                {
                    string s = filename.Substring(0, filename.LastIndexOf("\\"));
                    StringBuilder newfileName = new StringBuilder(s);
                    int index = whereSqlArr[i].LastIndexOf("=");
                    string sp = whereSqlArr[i].Substring(index + 2, whereSqlArr[i].Length - index - 3);
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
                            string sql = "select * from [" + tabelNameArray[j] + "]" + whereSqlArr[i];
                            IDataReader reader = DbConfig.Db.DbProvider.ExecuteReader(sql);
                            SaveExcel(package, sql, tabelNameArray[j]);

                        }

                        package.Save();
                    }



                }

                watch.Stop();
                return Convert.ToInt32(watch.ElapsedMilliseconds/1000);
            }

            return -1;
        }

        public async Task<int> ExportExcelAsync(string[] tabelNameArray, string filename, string[] whereSqlArr)
        {
            return await Task.Run(
                () => { return ExportExcel(tabelNameArray, filename, whereSqlArr); }
                );

        }
    }
}
