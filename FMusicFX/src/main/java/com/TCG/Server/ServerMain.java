package com.TCG.Server;

public class ServerMain {

    public static void main(String[] args){

        WebController server = new WebController();
        new Thread(server).run();
    }
}
