using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace SqlExport.Interface
{
    public interface IDbToExcel
    {
        /// <summary>
        /// 保存excel文件，覆盖相同文件名的文件
        /// </summary>
        void SaveExcel(string fileName, string sql, string sheetName);

        /// <summary>
        /// 工作簿中添加新的SHEET
        /// </summary>
        bool SaveExcel(ExcelPackage package, string sql, string sheetName);

        /// <summary>
        /// 单表格导出到一个EXCEL工作簿
        /// </summary>
        int ExportExcel(string fileName, string sql, string sheetName);

        /// <summary>
        /// 异步导出到一个EXCEL工作簿
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sql"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
         Task<int> ExportExcelAsync(string fileName, string sql, string sheetName);

        /// <summary>
        /// 多表格导出到一个EXCEL工作簿
        /// </summary>
        int ExportExcel(string[] tabelNameArray, string filename);

        Task<int> ExportExcelAsync(string[] tabelNameArray, string filename);

        /// <summary>
        /// 分拆导出
        /// </summary>
        int ExportExcel(string[] tabelNameArray, string filename, string[] whereSqlArr);

        Task<int> ExportExcelAsync(string[] tabelNameArray, string filename, string[] whereSqlArr);

    }
}
