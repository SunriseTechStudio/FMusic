using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FMusic.Util
{
    class NetWorkUtil
    {
        private static WebClient wc = new WebClient();
        public static string Token { set; get; } = "";

        public static JObject WebClientGet(string backYard)
        {
            return JObject.Parse(Encoding.UTF8.GetString(wc.DownloadData("http://cloud-music.pl-fe.cn/" + backYard)));
        }

        public static JObject HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Proxy = null;
            request.KeepAlive = false;
            request.Method = "GET";
            request.ContentType = "application/json; charset=UTF-8";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Headers["Cookie"] = "MUSIC_U=" + Token; 
            
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            myResponseStream.Close();

            if (!response.Equals(null)) response.Close();
            if (!request.Equals(null)) request.Abort();

            GC.Collect();
            return JObject.Parse(retString);
        }
    }
}
