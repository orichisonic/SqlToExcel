using System.Data;
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

     
    }
}
