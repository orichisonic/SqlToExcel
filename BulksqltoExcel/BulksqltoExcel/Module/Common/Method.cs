using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Text;

namespace BulksqltoExcel.Module.Common
{
    public static class Method
    {
        public static string ToJsonObject(IDictionary array)
        {
            StringBuilder jsonString = new StringBuilder();
            foreach (DictionaryEntry item in array)
            {
                jsonString.AppendFormat("\"{0}\":\"{1}\",", item.Key, item.Value);
            }
            if (jsonString.Length > 0)
            {
                return "{" + jsonString.ToString() + "}";
            }
            return "";
        }

        public static string ReplaceMessage(string message)
        {
            string mes = message.Replace(@"'", "|");
            mes = mes.Replace("\"", "|");
            mes = mes.Replace("\r", "");
            mes = mes.Replace("\n", "");
            return mes;
        }
    }
}