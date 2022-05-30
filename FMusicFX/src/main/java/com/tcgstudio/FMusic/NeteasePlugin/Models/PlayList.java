package com.tcgstudio.FMusic.NeteasePlugin.Models;

import java.util.ArrayList;

public class PlayList {
    public ArrayList<String> id = new ArrayList<>();
    public ArrayList<String> name = new ArrayList<>();
    public ArrayList<String> artist = new ArrayList<>();
    public ArrayList<String> musicImg = new ArrayList<>();

    public PlayList() {
    }

    public void AddId(String Id){id.add(Id);}

    public void AddName(String Name){name.add(Name);}

    public void AddArtist(String Artist){artist.add(Artist);}

    public void AddMusicIMG(String Url){musicImg.add(Url);}
}
