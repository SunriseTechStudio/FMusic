using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FMusic.Util.NetMusicCore
{
    public class AccountUtils
    {
        public static string userId = "";
        private Dictionary<string,string> userProfile = new Dictionary<string,string>();

        public AccountUtils(string username,string password)
        {
            JObject proot = NetWorkUtil.WebClientGet("login/cellphone?phone=" + username + "&password=" + password);
            JObject root = (JObject)proot["profile"];
            
            userProfile.Add("backgroundUrl", (string)root["backgroundUrl"]);
            userProfile.Add("avatarUrl", (string)root["avatarUrl"]);
            userProfile.Add("nickname", (string)root["nickname"]);
            userProfile.Add("signature", (string)root["signature"]);

            NetWorkUtil.Token = (string)proot["token"];
            userId = (string)root["userId"];
            GC.Collect();
        }

        public List<PlayList> GetMusicList()
        {
            JArray playlist = (JArray)NetWorkUtil.WebClientGet("user/playlist?uid=" + userId)["playlist"];
            List<PlayList> playLists = new List<PlayList>();

            foreach (JObject obj in playlist)
            {
                JArray tracks = (JArray)NetWorkUtil.HttpGet("http://cloud-music.pl-fe.cn/playlist/detail?id=" + obj["id"])["playlist"]["trackIds"];

                StringBuilder stb = new StringBuilder();
                foreach (JObject dataobj in tracks) stb.Append(dataobj["id"] + ",");
                
                JArray songs = (JArray)NetWorkUtil.HttpGet("http://music.163.com/api/song/detail/?id=1937930080&ids=[" + stb.ToString().Remove(stb.ToString().Length - 1) + "]&br=32000")["songs"];
                Dictionary<string, string> musics = new Dictionary<string, string>();

                try
                {
                    foreach (JObject song in songs) musics.Add( (string)song["id"] , (string)song["name"]);
                }
                catch { }

                playLists.Add(new PlayList((string)obj["coverImgUrl"], (string)obj["name"],userProfile["nickname"],(string)obj["playCount"],(string)obj["description"],musics));
            }
            GC.Collect();
            return playLists;
        }
    }

    public class PlayList
    {
        public string CoverImage;
        public string Title;
        public string Creator;
        public string playCount;
        public string introduce;
        public Dictionary<string, string> PlayLists = new Dictionary<string, string>();

        public PlayList(string ci,string tt,string ct,string pc,string it,Dictionary<string,string> pl)
        {
            CoverImage = ci;Title = tt;Creator = ct;playCount = pc;introduce = it;PlayLists = pl;
        }
    }
}
