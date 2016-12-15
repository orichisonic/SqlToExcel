using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace SqlToExcel.Module.Common
{
    public class RemotingServiceCommon
    {
        public static string RequestService(string url, string method, string param, string httpMethod, string contentType)
        {
            string result = "";
            try
            {
                string requestUrl = url + method;
                HttpWebRequest webRequest = HttpWebRequest.Create(requestUrl) as HttpWebRequest;
                webRequest.Method = httpMethod;
                webRequest.ContentType = contentType;
                Stream reqStream = webRequest.GetRequestStream();
                byte[] mybyte = System.Text.UTF8Encoding.UTF8.GetBytes(param);
                using (var stream = webRequest.GetRequestStream())
                {
                    stream.Write(mybyte, 0, mybyte.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webRequest.GetResponse()
                        .GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                result = "";
            }

            return result;
        }
    }
}
