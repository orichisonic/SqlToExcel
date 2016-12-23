using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace SqlExport.Interface
{
    public interface IDbToCsv
     {
         void WriteHeader(IDataReader reader, StreamWriter sw);

         void WriteContent(IDataReader reader, StreamWriter sw);

         int SaveCsv(IDataReader reader, string filename);

         Task<int> ExportCsvAsync(IDataReader reader, string filename);

         StreamWriter GetStreamWriter(string filename, int outCount);
         int SaveCsv(IDataReader reader, string filename, int pagesize);

         int ExportCsvAsync(IDataReader reader, string filename, int pagesize);
     }
}
