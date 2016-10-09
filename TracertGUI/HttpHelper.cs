using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TracertGUI
{
    public class HttpHelper
    {
        public static string APP_KEY = "f9d8bca18964";
        public static string GetResponse(string ip)
        {
            string url = "http://apicloud.mob.com/ip/query?key=" + APP_KEY + "&ip=" + ip + "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF8-";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string result = sr.ReadToEnd();
            stream.Close();
            sr.Close();
            return result;
        }
    }
}
