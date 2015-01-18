using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace TestWebApp.Models
{
    public static class MyUrl
    {
        public static string currentUrlChange(NameValueCollection param, Dictionary<string, string> changedAttributes)
        {
            string resultUrl = "?";
            foreach (string key in param.AllKeys)
            {
                if (!changedAttributes.ContainsKey(key))
                    changedAttributes[key] = param[key];
            }
            foreach (KeyValuePair<string, string> attr in changedAttributes)
            {
                resultUrl += String.Format("{0}={1}&", attr.Key, attr.Value);
            }

            return resultUrl.Remove(resultUrl.Length - 1);
        }
        public static string currentUrlChange(NameValueCollection param, params string[] attrs)
        {
            if (attrs.Length % 2 != 0)
                return null;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < attrs.Length; i += 2)
            {
                dict.Add(attrs[i], attrs[i + 1]);
            }
            return currentUrlChange(param, dict);
        }
    }
}