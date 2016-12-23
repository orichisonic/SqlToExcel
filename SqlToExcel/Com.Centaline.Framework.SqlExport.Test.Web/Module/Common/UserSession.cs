using System.Web;

namespace SqlToExcel.Module.Common
{
    public static class UserSession
    {
        public static void Login(UserLoginInfo userInfo)
        {
            if (userInfo!=null)
            {
                HttpContext.Current.Session[ConstValue.UserLoginSesstionKeyName] = userInfo;
            }
        }

        public static UserLoginInfo UserInfo
        {
            set
            {
                HttpContext.Current.Session[ConstValue.UserLoginSesstionKeyName] = value;
            }
            get
            {
                UserLoginInfo userInfo=null;
                if (HttpContext.Current.Session[ConstValue.UserLoginSesstionKeyName] != null)
                {
                    userInfo=HttpContext.Current.Session[ConstValue.UserLoginSesstionKeyName] as UserLoginInfo;
                }
                return userInfo;
            }
        }

        public static bool IsLogin()
        {
            return (HttpContext.Current.Session[ConstValue.UserLoginSesstionKeyName]!=null);
        }

        public static void LoginOut()
        {
            if (UserInfo != null)
            {
                HttpContext.Current.Session.Remove(ConstValue.UserLoginSesstionKeyName);
            }
        }

        public static bool LoginAd(string userName, string pwd)
        {
            bool result = true;
            string err = string.Empty;
            if (DomainValidateAd.ValidateAd(userName, pwd, out err))
            {
                Login(new UserLoginInfo("",userName));
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}