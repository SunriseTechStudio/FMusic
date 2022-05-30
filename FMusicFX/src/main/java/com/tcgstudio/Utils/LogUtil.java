package com.TCGStudio.Utils;

import java.text.SimpleDateFormat;
import java.util.Date;

public class LogUtil {
    private String logname;

    public LogUtil(String Logname){
        logname = Logname;
    }

    public void sendInfo(String Message){
        System.out.println(new StringBuilder().append(System.currentTimeMillis() + "|").append(logname).append(" [Info] ").append(Message).toString());
    }
}
