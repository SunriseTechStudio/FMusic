package com.tcgstudio.FMusicMidiUtils;
public class FMidiNode{
    private int length;
    private String player;
    private int absNode;

    public FMidiNode(int absNode,int length,String player){
        this.absNode=absNode;
        this.length=length;
        this.player=player;
    }
    public void play(){
       // double freq=Frequtil.
    }
}