using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMusic.Util.NetMusicCore
{
    class MusicDetil
    {
        private string id;
        private string name,picurl,musicUrl;
        public MusicDetil(string id)
        {
            this.id = id;
            JObject root = NetWorkUtil.HttpGet("http://music.163.com/api/song/detail/?id=1937930080&ids=[" + id + "]&br=32000");
            name = (string)root["songs"][0]["name"];
            picurl = (string)root["songs"][0]["album"]["picUrl"];
            musicUrl = NetWorkUtil.HttpGet("http://music.163.com/api/song/enhance/player/url?id=123456&ids=[" + id + "]&br=3200000")["data"][0]["url"].ToString();
        }

        public string GetMusicUrl()
        {
            return musicUrl;
        }

        public string GetMusicPicUrl()
        {
            return picurl;
        }

        public Dictionary<string,string> GetMusicArtistAndID()
        {
            JObject root = NetWorkUtil.HttpGet("http://music.163.com/api/song/detail/?id=1937930080&ids=[" + id + "]&br=32000");
            JArray artists = ((JArray)root["songs"][0]["artists"]);
            Dictionary<string,string> namse = new Dictionary<string,string>();
            foreach (JObject obj in artists) namse.Add((string)obj["name"], (string)obj["id"]);
            return namse;
        }

        public string getName()
        {
            return name;
        }
    }
}
