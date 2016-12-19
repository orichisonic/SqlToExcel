using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace SqlExport.Interface
{
    public  interface IDataTabletoExcel
    {
         DataTable GetDataTableFromSQL(string sql);

        DataTable GetDataTableFromName(string TabalName);

        bool SaveExcel(string SheetName, DataTable dt, ExcelPackage package);

        void SaveExcel(string FileName, DataTable dt, string NewSheetName);

        int ExportTemplate(string TabelName, string filename);

        int ExportExcel(string SheetName, string sql, string filename);

        int ExportExcel(string TabelName, int PageSize, string filename);

        int ExportExcel(IList<string> TabelNames, string filename);
    }
}
