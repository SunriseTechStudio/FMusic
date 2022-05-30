package com.tcgstudio.FMusic.NeteasePlugin;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import com.tcgstudio.FMusic.NeteasePlugin.Models.MusicItem;
import com.tcgstudio.Net.WebUtil;

import java.util.ArrayList;

public class GetMusicIDUtil {
    public static ArrayList<MusicItem> items = new ArrayList<>();

    public static void SearchMusic(String Text) throws Exception{
        items.clear();
        JSONObject obj = JSONObject.parseObject(WebUtil.GetWeb("http://cloud-music.pl-fe.cn/search?keywords=" + Text));
        
        JSONArray songs = obj.getJSONObject("result").getJSONArray("songs");
        for(Object item : songs){
            StringBuilder what = new StringBuilder();
            JSONObject j1 = (JSONObject) item;
            JSONArray art = j1.getJSONArray("artists");
            for(Object objdta : art){
                what.append(((JSONObject)objdta).getString("name"));
            }

            items.add(new MusicItem(j1.getString("name"),what.toString(),j1.getString("id")));
        }
    }

}

