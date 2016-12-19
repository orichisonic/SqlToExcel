using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExport.DBUtility.Interface
{
    public interface IDbUtility
    {
        #region 执行SQL操作     

        DbHelperSql GetConnectstring(string connectString);

        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="sqlString"></param>
        int ExecuteSql(string sqlString);
        int TruncateTable(string tableName);
        IDataReader ExecuteReader(string strSql);

        #endregion

        #region 返回DataTable对象

        /// <summary>
        /// 运行SQL语句,返回DataTable对象
        /// </summary>
        DataTable ReturnDataTable(string sql, int startIndex, int pageSize);
        /// <summary>
        /// 运行SQL语句,返回DataTable对象
        /// </summary>
        DataTable ReturnDataTable(string sql);
        #endregion

        #region 存储过程操作
        int RunProcedure(string storedProcName);
        #endregion

        #region 获取数据库Schema信息
        /// <summary>
        /// 获取SQL SERVER中数据库列表
        /// </summary>
        IList<string> GetDataBaseInfo();
        IList<string> GetTableInfo();
        IList<string> GetColumnInfo(string tableName);
        IList<string> GetProcInfo();
        //IList<string> GetFunctionInfo();
        IList<string> GetViewInfo();
        int ReturnTbCount(string tbName);
        #endregion

        #region 批量导入数据库
        /// <summary>
        /// 批量导入数据库
        /// </summary>
        bool DatatableImport(IList<string> maplist, string tableName, DataTable dt);
        int BulkCopyFromOpenrowset(IList<string> maplist, string tableName, string filename);
        #endregion
    }
}
