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
        DataTable GetDataTableFromSql(string sql);

        DataTable GetDataTableFromName(string tableName);

        bool SaveExcel(string sheetName, DataTable dt, ExcelPackage package);

        void SaveExcel(string fileName, DataTable dt, string newSheetName);

        int ExportTemplate(string tabelName, string filename);

        int ExportExcel(string sheetName, string sql, string filename);

        int ExportExcel(string tabelName, int pageSize, string filename);

      

        int ExportExcel(IList<string> tabelNames, string filename);
    }
}
