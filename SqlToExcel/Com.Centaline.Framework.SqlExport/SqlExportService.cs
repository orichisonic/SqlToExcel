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

        public virtual IDBUtility DBUtility { get; set; }

        public virtual IDbToExcel DbToExcel { get; set; }

        public virtual IDbToCsv DbToCsv { get; set; }

        public virtual IDataTabletoExcel DataTabletoExcel { get; set; }

        public Entity.Result<List<IDictionary<string, string>>> Query(HttpRequestBase request)
        {
            string errorMessage = null;
            string json = null;
            SqlExport.Entity.PageResult<List<IDictionary<string, string>>> queryResult = new SqlExport.Entity.PageResult<List<IDictionary<string, string>>>();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string sqlstatement = request["sqlstatement"];
            string ProviderName = request["ProviderName"];
            string ServerName = request["ServerName"];
            string ValidataType = request["ValidataType"];
            string UserName = request["UserName"];
            string UserPwd = request["UserPwd"];
            string DataBase = request["DataBase"];
            //string ConString = request["ConString"];
            //string DBProvider = request["DBProvider"];
            int pageIndex = int.Parse(request["pageIndex"]);
            int pageSize = int.Parse(request["pageSize"]);
            string fileName = request["fileName"];
            DBConfig.db.ProviderName = "SQL";
            DBConfig.db.DataBase = DataBase;
            string _conString = DBConfig.db.GetConstring(ProviderName, ValidataType, UserName, UserPwd, DataBase, ServerName);
            DBConfig.db.DBProvider = DBUtility.GetConnectstring(_conString);
            string strSql =
                "select * from (select ROW_NUMBER() over({0}) RowNum,*from({1}) T1 ) T2 where RowNum > {2} and RowNum<= {3}";
            sqlstatement= string.Format(strSql,"order by levels", sqlstatement,(pageIndex-1)*pageSize, pageIndex*pageSize);
            DataTable dt=DBConfig.db.DBProvider.ReturnDataTable(sqlstatement);
           
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
                queryResult.PageInfo.PageCount = 10;
            }
            return queryResult;
        }

        public Entity.Result<List<IDictionary<string, string>>> ExportToCsv(HttpRequestBase request)
        {
            string errorMessage = null;
            string json = null;
            Entity.Result<List<IDictionary<string, string>>> queryResult = new Entity.Result<List<IDictionary<string, string>>>();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string sqlstatement = request["sqlstatement"];
            string ProviderName = request["ProviderName"];
            string ServerName = request["ServerName"];
            string ValidataType = request["ValidataType"];
            string UserName = request["UserName"];
            string UserPwd = request["UserPwd"];
            string DataBase = request["DataBase"];
            //string ConString = request["ConString"];
            //string DBProvider = request["DBProvider"];
            //string pageIndex = request["pageIndex"];
            //string pageSize = request["pageSize"];
            string fileName = request["fileName"];
            DBConfig.db.ProviderName = "SQL";
            DBConfig.db.DataBase = DataBase;
            string _conString = DBConfig.db.GetConstring(ProviderName,ValidataType,UserName,UserPwd,DataBase,ServerName);
            DBConfig.db.DBProvider = DBUtility.GetConnectstring(_conString);

            int ret= ExportCSVAsync(fileName, sqlstatement);
            queryResult.Code = ret;

            if (ret > 0)
            {
                queryResult.Message = "分页导出csv成功";
            }
            return queryResult;
        }

        public Entity.Result<List<IDictionary<string, string>>> ExportToExcel(HttpRequestBase request)
        {
            string errorMessage = null;
            string json = null;
            Entity.Result<List<IDictionary<string, string>>> queryResult = new Entity.Result<List<IDictionary<string, string>>>();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string sqlTableName = request["sqlTableName"];
            string ProviderName = request["ProviderName"];
            string ServerName = request["ServerName"];
            string ValidataType = request["ValidataType"];
            string UserName = request["UserName"];
            string UserPwd = request["UserPwd"];
            string DataBase = request["DataBase"];
            //string ConString = request["ConString"];
            //string DBProvider = request["DBProvider"];
            //string pageIndex = request["pageIndex"];
            //string pageSize = request["pageSize"];
            string fileName = request["fileName"];
            DBConfig.db.ProviderName = "SQL";
            DBConfig.db.DataBase = DataBase;
            //string _conString = DBConfig.db.GetConstring(ProviderName, ValidataType, UserName, UserPwd, DataBase, ServerName);
            //DBConfig.db.DBProvider = DBUtility.GetConnectstring(_conString);

            //int ret = ExportCSVAsync(fileName, sqlstatement);
            int ret = DataTabletoExcel.ExportExcel(sqlTableName, 65535, fileName);
            queryResult.Code = ret;

            if (ret > 0)
            {
                queryResult.Message = "分页导出excel成功";
            }
            return queryResult;
        }



        public  int ExportCSVAsync( string filename, string sql)
        {

            try
            {
                IDataReader reader = DBConfig.db.DBProvider.ExecuteReader(sql);
                if (filename != null)
                {
                  return  DbToCsv.ExportCsvAsync(reader, filename, ExportConfig.RowOutCount);
                }
                else
                {

                    return 0;
                }
                
              
                //MessageBox.Show("导数已完成！");
                GC.Collect();

            }
            catch (Exception ee)
            {

                return 0;
            }



        }
    }
}
