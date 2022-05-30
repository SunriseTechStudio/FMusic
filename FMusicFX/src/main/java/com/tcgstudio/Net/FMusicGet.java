package com.tcgstudio.Net;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONObject;

import java.net.URLDecoder;

public class FMusicGet {
    public static JSONObject GetBack(String url) throws Exception{
        return JSON.parseObject(URLDecoder.decode(WebUtils.sendGet(url),"utf-8"));
    }
}
