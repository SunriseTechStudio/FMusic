package com.tcgstudio.FMusic.NeteasePlugin.Models;

public class MusicItem {
    public String Name;
    public String Artist;
    public String Url;

    public MusicItem(String... Args) {
        Name = Args[0];
        Artist = Args[1];
        Url = Args[2];
    }
}
