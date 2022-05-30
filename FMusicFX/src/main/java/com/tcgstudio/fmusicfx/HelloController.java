package com.tcgstudio.fmusicfx;

import com.tcgstudio.FMusic.NeteasePlugin.Models.MusicItem;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import com.tcgstudio.FMusic.NeteasePlugin.*;
import javafx.scene.control.ListView;
import javafx.scene.control.Slider;
import javafx.scene.control.TextField;
import javafx.scene.input.MouseEvent;
import javafx.scene.media.Media;
import javafx.scene.media.MediaPlayer;
import javafx.scene.media.MediaView;
import javafx.util.Duration;

import java.util.ArrayList;

public class HelloController {

    @FXML
    private TextField title;
    @FXML
    private ListView listView;
    @FXML
    private MediaView Player;
    @FXML
    private Slider slider;

    private ArrayList<String> name = new ArrayList<>();
    private ArrayList<String> link = new ArrayList<>();

    public void SearchClick(){

        try {
            name.clear();link.clear();
            GetMusicIDUtil.SearchMusic(title.getText());

            for(MusicItem item : GetMusicIDUtil.items){
                name.add(item.Name);link.add(item.Url);
            }
            ObservableList<?> obj = FXCollections.observableList(name);
            listView.setItems(obj);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private MediaPlayer mediaPlayer;
    private boolean isplaying = false;

    public void OnPlayClick() throws Exception{
        var getMusic = new GetMusicUtil(link.get(listView.getSelectionModel().getSelectedIndex()));

        Media media1 =new Media(getMusic.GetSoundPlayUri());
        if(mediaPlayer != null){
            mediaPlayer.stop();
            mediaPlayer.dispose();
        }
        mediaPlayer = new MediaPlayer(media1);
        mediaPlayer.play();

        mediaPlayer.currentTimeProperty().addListener(ov -> {
            double currentTime = mediaPlayer.getCurrentTime().toSeconds();
            //lbCurrentTime.setText(Seconds2Str(currentTime)+"/"+Seconds2Str(endTime));
            if(!isDraging) slider.setValue(currentTime/mediaPlayer.getStopTime().toSeconds()*100);
        });

        mediaPlayer.setOnEndOfMedia(new Runnable() {
            @Override
            public void run() {
                isplaying = false;
                mediaPlayer.dispose();
                System.gc();
            }
        });
        mediaPlayer.setOnReady(new Runnable(){

            @Override
            public void run() {
                isplaying = true;
            }

        });
        Player.setMediaPlayer(mediaPlayer);
    }
    private boolean isDraging = false;

    public void PlayStop(ActionEvent actionEvent) {
        if(isplaying){mediaPlayer.pause();isplaying = false;}
        else {mediaPlayer.play();isplaying = true;}
    }

    public void dragEvent(MouseEvent mouseEvent) {
        isDraging = true;
    }

    public void MusicGoTo(MouseEvent mouseEvent) {
        double dob = slider.getValue() / slider.getMax() * mediaPlayer.getStopTime().toSeconds();
        System.out.println(dob);
        mediaPlayer.seek(Duration.seconds(dob));
        isDraging = false;
    }
}