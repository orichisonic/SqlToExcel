using System.Data;
using System.Diagnostics;
using System.Web;
using SqlExport.Config;
using SqlExport.DBUtility.Interface;
using SqlExport.Interface;

namespace SqlExport
{
    public class SqlExportService : ISqlExportService
    {

        public virtual IDbUtility DbUtility { get; set; }

        public virtual IDataTabletoExcel DataTabletoExcel { get; set; }

    
        public DataTable GetDataTableFromSql(HttpRequestBase request, string sql)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string sqlTableName = request["Sheetname"];
            string providerName = ConfigInfo.ProviderName;
            string serverName = ConfigInfo.ServerName;
            string validataType = ConfigInfo.ValidateType;
            string userName = ConfigInfo.UserName;
            string userPwd = ConfigInfo.UserPwd;
            string dataBase = ConfigInfo.DataBase;
            string fileName = request["fileName"];
            //string sql = "SELECT  [UserID],[UserCode],[UserName],[ParentID],[Position],[Mobile],[Email],[Levels],[AttentionTime]FROM Users WHERE CreateStatus = 1 AND(AttentionState = 1); ";

            DbConfig.Db.ProviderName = ConfigInfo.ProviderName;
            DbConfig.Db.DataBase = ConfigInfo.DataBase;
            string conString = DbConfig.Db.GetConstring(providerName, validataType, userName, userPwd, dataBase, serverName);
            DbConfig.Db.DbProvider = DbUtility.GetConnectstring(conString);
            return DataTabletoExcel.GetDataTableFromSql(sql);
        }
    }
}
