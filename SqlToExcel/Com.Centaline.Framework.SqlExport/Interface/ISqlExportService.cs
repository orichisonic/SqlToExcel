using System.Collections.Generic;
using System.Data;
using System.Web;
using SqlExport.Entity;

namespace SqlExport.Interface
{
    public interface ISqlExportService
    {
       DataTable GetDataTableFromSql(HttpRequestBase request, string sql);
    }
}
