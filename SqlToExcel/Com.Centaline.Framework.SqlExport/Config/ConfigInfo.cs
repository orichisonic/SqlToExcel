using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SqlExport.Config
{
    public class ConfigInfo
    {


        public static string ServerName
        {
            get
            {
                 return System.Configuration.ConfigurationManager.AppSettings["ServerName"];
            }
        }

        public static string UserName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UserName"];
            }
        }

        public static string UserPwd
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UserPwd"];
            }
        }



        public static string DataBase
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DataBase"];
            }
        }

        public static string ValidateType
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ValidateType"];
            }
        }


        public static string ProviderName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ProviderName"];
            }
        }
    }
}
