using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Helper
{
    public class HTTPHelper
    {
        public static Dictionary<string, string> ParseUrlQueryString(string data)
        {
            int startIndex = data.IndexOf("?") + 1;
            int endIndex = data.IndexOf(" HTTP");
            data = data.Substring(startIndex, endIndex - startIndex);

            string[] querys = data.Split('&');
            Dictionary<string, string> queryDic = new Dictionary<string, string>();
            for (int i = 0; i < querys.Length; i++)
            {
                var pair = querys[i].Split('=');
                queryDic.Add(pair[0], pair[1]);
            }
            return queryDic;
        }
    }
}
