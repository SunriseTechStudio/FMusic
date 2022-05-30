package com.tcgstudio.FMusicMidiUtils;

public class Frequtil {
    public static double calcFreq(int relativeNode){
        return(440/32.)*Math.pow(2.,((relativeNode-9.)/12));
    }
}
