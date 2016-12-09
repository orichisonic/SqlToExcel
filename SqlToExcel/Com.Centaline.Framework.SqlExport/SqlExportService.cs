using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SqlExport.DBUtility.Interface;
using SqlExport.Entity;
using SqlExport.Interface;

namespace SqlExport
{

    public class SqlExportService:ISqlExportService
    {

        public virtual IDBUtility DBUtility { get; set; }

        public virtual IDbToDestination DbToDestination { get; set; }



        public Result<List<IDictionary<string, string>>> Query(HttpRequestBase request)
        {
            string errorMessage = null;
            string json = null;
            Result<List<IDictionary<string, string>>> queryResult = new Result<List<IDictionary<string, string>>>();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string sqlstatement = request["sqlstatement"];
            string ProviderName = request["ProviderName"];
            string ServerName = request["ServerName"];
            string ValidataType = request["ValidataType"];
            string UserName = request["UserName"];
            string UserPwd =request["UserPwd"];
            string DataBase =request["DataBase"];
            string ConString =request["ConString"];
            string DBProvider = request["DBProvider"];
            return queryResult;
        }
    }
}
