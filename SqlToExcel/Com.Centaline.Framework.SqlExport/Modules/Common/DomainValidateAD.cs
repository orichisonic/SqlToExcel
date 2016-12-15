using System;
using System.Collections.Generic;
using System.Web;
using SqlExport.Modules.Common;

namespace SqlExport.Modules.Common
{
    public class DomainValidateAD
    {
        public static bool ValidateAD(string userID, string password, out string err)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>(2);
            parms.Add("UserID", userID);
            parms.Add("Password", password);
            err = RemotingServiceCommon.RequestService(ConfigInfo.ADLoginUrl, string.Empty, Method.ToJsonObject(parms), "POST", "application/json");
            if (err.IndexOf("ErrorCode") == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}