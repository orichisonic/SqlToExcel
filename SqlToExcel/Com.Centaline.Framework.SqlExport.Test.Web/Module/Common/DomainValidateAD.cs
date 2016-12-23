using System.Collections.Generic;


namespace SqlToExcel.Module.Common
{
    public class DomainValidateAd
    {
        public static bool ValidateAd(string userId, string password, out string err)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>(2);
            parms.Add("UserID", userId);
            parms.Add("Password", password);
            err = RemotingServiceCommon.RequestService(ConfigInfo.AdLoginUrl, string.Empty, Method.ToJsonObject(parms), "POST", "application/json");
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