using System.Collections.Generic;
using System.Web;
using SqlExport.Entity;

namespace SqlExport.Interface
{
    public interface ISqlExportService
    {
        Result<List<IDictionary<string, string>>> ExportToCsv(HttpRequestBase request);

        Result<List<IDictionary<string, string>>> ExportToExcel(HttpRequestBase request,string sql);

        Result<List<IDictionary<string, string>>> Query(HttpRequestBase request);
    }
}
