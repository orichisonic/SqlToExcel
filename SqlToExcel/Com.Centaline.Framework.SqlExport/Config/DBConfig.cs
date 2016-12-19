using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExport.Config
{
    public class DbConfig
    {
        /// <summary>
        /// 数据库配置类
        /// </summary>
     

            public static DbConfig Db = new DbConfig();


            public DbConfig()
            {

            }

            private string _providerName;
            /// <summary>
            ///数据提供程序
            /// </summary>
            public string ProviderName
            {
                get { return _providerName; }
                set { _providerName = value; }
            }

            private string _serverName;
            /// <summary>
            /// 服务器名
            /// </summary>
            public string ServerName
            {
                get { return _serverName; }
                set { _serverName = value; }
            }

            private string _validataType;
            /// <summary>
            /// 验证类型
            /// </summary>
            public string ValidataType
            {
                get { return _validataType; }
                set { _validataType = value; }
            }
            private string _userName;
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName
            {
                get { return _userName; }
                set { _userName = value; }
            }
            private string _userPwd;
            /// <summary>
            /// 密码
            /// </summary>
            public string UserPwd
            {
                get { return _userPwd; }
                set { _userPwd = value; }
            }
            private string _dataBase;
            /// <summary>
            /// 数据库
            /// </summary>
            public string DataBase
            {
                get { return _dataBase; }
                set { _dataBase = value; }
            }

            private string _conString;
            /// <summary>
            /// 连接字符串
            /// </summary>
            public string ConString
            {
                get { return _conString; }
                set { _conString = value; }
            }
            private SqlExport.DBUtility.Interface.IDbUtility _dbProvider;
            /// <summary>
            /// 数据访问对象
            /// </summary>
            public SqlExport.DBUtility.Interface.IDbUtility DbProvider
            {
                get { return _dbProvider; }
                set { _dbProvider = value; }
            }

            public string GetSqLmasterConstring()
            {
                if (Db.ProviderName == "SQL")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Data Source=" + Db.ServerName);
                    sb.Append(";Initial Catalog=master ;");
                    if (Db.ValidataType == "Windows身份认证")
                    {
                        sb.Append(" Integrated Security=SSPI;");
                    }
                    else
                    {

                        sb.Append("User ID=" + Db.UserName + ";Password=" + Db.UserPwd + ";");

                    }
                    return sb.ToString();
                }
                return "";
            }

            public string GetConstring(string ProviderName,string ValidataType,string UserName,string UserPwd,string DataBase,string ServerName)
            {
                if (ProviderName == "SQL")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Data Source=" + ServerName);
                    sb.Append(";Initial Catalog=" + DataBase + " ; ");
                    if (ValidataType == "Windows身份认证")
                    {
                        sb.Append("Integrated Security=SSPI;Connect Timeout=10000");
                    }
                    else
                    {

                        sb.Append("User ID=" + UserName + ";Password=" + UserPwd + ";");

                    }
                    return sb.ToString();
                }
                else if (ProviderName == "ACC")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Provider=Microsoft.Ace.OleDb.12.0");
                    sb.Append(";Data Source= " + DataBase);
                    sb.Append(";Persist Security Info=False;");
                    return sb.ToString();
                }
                else if (ProviderName == "SQLite")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Data Source= " + DataBase + ";");
                    return sb.ToString();
                }
                else return "";

            }



        }


    
}
