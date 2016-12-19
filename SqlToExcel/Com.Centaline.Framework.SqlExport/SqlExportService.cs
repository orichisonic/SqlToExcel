using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Com.Centaline.Framework.QuickQuery.Entity;
using SqlExport.Config;
using SqlExport.DBUtility.Interface;
using SqlExport.Interface;

namespace SqlExport
{
    public class SqlExportService : ISqlExportService
    {

        public virtual IDbUtility DbUtility { get; set; }

        public virtual IDbToExcel DbToExcel { get; set; }

        public virtual IDbToCsv DbToCsv { get; set; }

        public virtual IDataTabletoExcel DataTabletoExcel { get; set; }

        public Entity.Result<List<IDictionary<string, string>>> Query(HttpRequestBase request)
        {
            SqlExport.Entity.PageResult<List<IDictionary<string, string>>> queryResult = new SqlExport.Entity.PageResult<List<IDictionary<string, string>>>();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string sqltable = request["sqltable"];
            string providerName = request["ProviderName"];
            string serverName = request["ServerName"];
            string validataType = request["ValidataType"];
            string userName = request["UserName"];
            string userPwd = request["UserPwd"];
            string dataBase = request["DataBase"];
            int pageIndex = int.Parse(request["pageIndex"]);
            int pageSize = int.Parse(request["pageSize"]);
            string fileName = request["fileName"];
            DbConfig.Db.ProviderName = "SQL";
            DbConfig.Db.DataBase = dataBase;
            string conString = DbConfig.Db.GetConstring(providerName, validataType, userName, userPwd, dataBase, serverName);
            DbConfig.Db.DbProvider = DbUtility.GetConnectstring(conString);
            string strSql =
                "select * from (select ROW_NUMBER() over({0}) RowNum,*from({1}) T1 ) T2 where RowNum > {2} and RowNum<= {3}";
            string sqlstatement= string.Format(strSql,"order by levels", "select * from "+sqltable, (pageIndex-1)*pageSize, pageIndex*pageSize);
            DataTable dt=DbConfig.Db.DbProvider.ReturnDataTable(sqlstatement);
           
            if(dt.Rows.Count>0)
            { 
            queryResult.Code = 2;

            List<IDictionary< string, string>> result=new List<IDictionary<string, string>>();
            if (null != dt && dt.Rows.Count > 0)
            {
                IDictionary<string, string> rowData = null;
                foreach (DataRow row in dt.Rows)
                {
                    rowData = new Dictionary<string, string>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        rowData.Add(column.ColumnName, row[column.ColumnName].ToString());
                    }
                    result.Add(rowData);
                }
            }
            queryResult.Data = result;
            queryResult.Message = "分页显示";
            int count = DbUtility.ReturnTbCount(sqltable);
            queryResult.PageInfo.PageCount = count/10;
            }
            return queryResult;
        }

        public Entity.Result<List<IDictionary<string, string>>> ExportToCsv(HttpRequestBase request)
        {
            Entity.Result<List<IDictionary<string, string>>> queryResult = new Entity.Result<List<IDictionary<string, string>>>();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string sqlstatement = request["sqlstatement"];
            string providerName = request["ProviderName"];
            string serverName = request["ServerName"];
            string validataType = request["ValidataType"];
            string userName = request["UserName"];
            string userPwd = request["UserPwd"];
            string dataBase = request["DataBase"];
            string fileName = request["fileName"];
            DbConfig.Db.ProviderName = "SQL";
            DbConfig.Db.DataBase = dataBase;
            string conString = DbConfig.Db.GetConstring(providerName,validataType,userName,userPwd,dataBase,serverName);
            DbConfig.Db.DbProvider = DbUtility.GetConnectstring(conString);

            int ret= ExportCsvAsync(fileName, sqlstatement);

            queryResult.Code = ret;

            if (ret > 0)
            {
                queryResult.Message = "分页导出csv成功";
            }
            return queryResult;
        }
        public int ExportCsvAsync(string filename, string sql)
        {

            try
            {
                IDataReader reader = DbConfig.Db.DbProvider.ExecuteReader(sql);
                if (filename != null)
                {
                    return DbToCsv.ExportCsvAsync(reader, filename, ExportConfig.RowOutCount);
                }
                else
                {
                    return 0;
                }
                GC.Collect();
            }
            catch (Exception ee)
            {
                return 0;
            }
        }

        public Entity.Result<List<IDictionary<string, string>>> ExportToExcel(HttpRequestBase request)
        {
            Entity.Result<List<IDictionary<string, string>>> queryResult = new Entity.Result<List<IDictionary<string, string>>>();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string sqlTableName = request["sqlTableName"];
            string providerName = request["ProviderName"];
            string serverName = request["ServerName"];
            string validataType = request["ValidataType"];
            string userName = request["UserName"];
            string userPwd = request["UserPwd"];
            string dataBase = request["DataBase"];
            string fileName = request["fileName"];
            DbConfig.Db.ProviderName = "SQL";
            DbConfig.Db.DataBase = dataBase;
            int ret = DataTabletoExcel.ExportExcel(sqlTableName, 65535, fileName);
            queryResult.Code = ret;
            if (ret > 0)
            {
                queryResult.Message = "分页导出excel成功";
            }
            return queryResult;
        }



     
    }
}
