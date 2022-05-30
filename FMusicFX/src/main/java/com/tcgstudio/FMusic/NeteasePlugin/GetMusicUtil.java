package com.tcgstudio.FMusic.NeteasePlugin;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONObject;
import com.tcgstudio.Net.FMusicGet;

public class GetMusicUtil {
    private String id;

    public GetMusicUtil(String id){
        this.id = id;
    }

    public String GetSoundPlayUri() throws Exception{
        JSONObject obj = FMusicGet.GetBack("http://music.163.com/api/song/enhance/player/url?id=123456&ids=["+id+"]&br=3200000");
        JSONObject ary = (JSONObject) obj.getJSONArray("data").get(0);
        return ary.getString("url");
    }

    public String GetSoundLyric() throws Exception{
        JSONObject obj = FMusicGet.GetBack("http://music.163.com/api/song/lyric?os=pc&id=" + id + "&lv=-1&kv=-1&tv=-1");
        return ((JSONObject)obj.get("lrc")).getString("lyric");
    }

    public String GetSoundChineseLyric() throws Exception{
        JSONObject obj = FMusicGet.GetBack("http://music.163.com/api/song/lyric?os=pc&id=" + id + "&lv=-1&kv=-1&tv=-1");
        return ((JSONObject)obj.get("tlyric")).getString("lyric");
    }
}
