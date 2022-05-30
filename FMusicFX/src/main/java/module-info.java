module com.tcgstudio.fmusicfx {
    requires javafx.controls;
    requires javafx.fxml;
    requires fastjson;
    requires httpclient;
    requires httpcore;
    requires javafx.media;


    opens com.tcgstudio.fmusicfx to javafx.fxml;
    exports com.tcgstudio.fmusicfx;
}