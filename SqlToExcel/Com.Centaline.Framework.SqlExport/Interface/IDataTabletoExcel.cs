using System.Collections.Generic;
using System.Data;


namespace SqlExport.Interface
{
    public  interface IDataTabletoExcel
    {
        DataTable GetDataTableFromSql(string sql);

        DataTable GetDataTableFromName(string tableName);

  
    }
}
