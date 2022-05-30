package com.tcgstudio.FMusic.NeteasePlugin;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import com.tcgstudio.FMusic.NeteasePlugin.Models.MUSICList;
import com.tcgstudio.FMusic.NeteasePlugin.Models.PlayList;
import com.tcgstudio.Net.FMusicGet;

import java.util.ArrayList;

public class UserUtils {
    private String username,password;
    public String Token;
    public String ProfileImage,Profileavstart,ProfileNickname,ProfileDesp,UserID;

    public UserUtils(String... Args){
        username = Args[0];
        password = Args[1];
    }

    public boolean Login() {
        try {
            JSONObject obj = FMusicGet.GetBack("http://cloud-music.pl-fe.cn/login/cellphone?phone=" + username + "&password=" + password);

            JSONObject profile = obj.getJSONObject("profile");
            Token = obj.getString("token");
            ProfileImage = profile.getString("backgroundUrl");
            Profileavstart = profile.getString("avatarUrl");
            ProfileNickname = profile.getString("nickname");
            ProfileDesp = profile.getString("signature");
            UserID = profile.getString("userId");
            return true;
        }catch (Exception e){
            return false;
        }
    }

    public ArrayList<MUSICList> playLists() throws Exception{
        ArrayList<MUSICList> musicLists = new ArrayList<>();
        JSONObject proot = FMusicGet.GetBack("http://cloud-music.pl-fe.cn/user/playlist?uid=" + UserID);
        JSONArray playlist = proot.getJSONArray("playlist");

        for (Object obj : playlist){
            JSONObject object = (JSONObject)obj;
            musicLists.add(new MUSICList(object.getString("id"),object.getString("name"),object.getString("coverImgUrl")));
        }
        return musicLists;
    }

    /*

     */
    public PlayList GetPlayList(MUSICList musicList) throws Exception{
        JSONObject root = FMusicGet.GetBack("http://cloud-music.pl-fe.cn/playlist/detail?id=" + musicList.id);
        JSONArray tracks = (root.getJSONObject("playlist")).getJSONArray("trackIds");

        //args.DetilOfPlaylist.Add(new string[] { root1["playlist"]["description"].ToString(), root1["playlist"]["playCount"].ToString(), root1["playlist"]["subscribedCount"].ToString() });

        PlayList playList = new PlayList();

        for(Object obj : tracks){
            playList.AddId(((JSONObject)obj).getString("id"));
        }

        StringBuilder idBuilder = new StringBuilder();
        for(String id : playList.id) idBuilder.append(id + ",");

        idBuilder = idBuilder.replace(idBuilder.length() - 2,idBuilder.length() - 1,",");

        JSONObject idS = FMusicGet.GetBack("http://cloud-music.pl-fe.cn/song/detail?ids=" + idBuilder.toString());
        JSONArray idSS = idS.getJSONArray("songs");

        for(Object obj : idSS){
            JSONObject jsonObject = (JSONObject) obj;
            playList.AddName(jsonObject.getString("name"));
            playList.AddMusicIMG(jsonObject.getJSONObject("al").getString("picUrl"));
            JSONArray ar = jsonObject.getJSONArray("ar");
            StringBuilder stringBuilder = new StringBuilder();
            for(Object name : ar) stringBuilder.append(((JSONObject)name).getString("name") + " ");
            playList.AddArtist(stringBuilder.toString());
        }
        System.gc();
        return playList;
    }

  //  public void 
}
