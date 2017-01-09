using System.Web;

namespace SqlToExcel.Module.Common
{
    public static class ConfigInfo
    {
        public static int CanVoteCheck
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings["CanVoteCheck"]); }
        }
        
        public static string AdLoginUrl
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ADLogin"]; }
        }

        public static string DbPath
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["DbPath"]; }
        }

        public static int PageSize
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"]); }
        }

        public static int MaxVoteCount
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxVoteCount"]); }
        }

        public static string DbConnectionStr
        {
            get 
            { 
                return string.Format(System.Configuration.ConfigurationManager.AppSettings["DbConnectionStr"],HttpContext.Current.Server.MapPath(DbPath)); 
            }
        }

        public static string WhiteList
        {
            get
            {
                return string.Format(System.Configuration.ConfigurationManager.AppSettings["WhiteList"], HttpContext.Current.Server.MapPath(DbPath));
            }
        }


    }
}