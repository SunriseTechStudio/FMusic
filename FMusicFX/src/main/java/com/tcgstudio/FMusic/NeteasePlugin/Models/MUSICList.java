package com.tcgstudio.FMusic.NeteasePlugin.Models;

public class MUSICList {
    public String id;
    public String name;
    public String url;

    public MUSICList(String... args){
        id= args[0];name = args[1];url = args[2];
    }
}
