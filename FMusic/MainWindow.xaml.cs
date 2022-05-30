using FMusic.Pages;
using FMusic.Util;
using FMusic.Util.NetMusicCore;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FMusic
{
    public partial class MainWindow : Window
    {
        private MediaElement mediaPlayer = new MediaElement();

        public MainWindow()
        {
            InitializeComponent();
            Background.Children.Add(mediaPlayer);
            mediaPlayer.LoadedBehavior = MediaState.Manual;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;

            GC.Collect();
            PlayListPage.PlayMusicDefult += PlayListPage_PlayMusicDefult;
            LoginThread();

            //addToPlayList("1806768577");
            //addToPlayList("1937930080");
        }

        private void PlayListPage_PlayMusicDefult(int Index, PlayList play)
        {
            addToPlayList(play, Index);
        }

        public static List<MusicItem> PlayList { set; get; } = new List<MusicItem>();

        #region 播放器源代码
        private bool isThumbDraging = false,isPlaying = false;
        private int playingType = 0,playIndex = 0;
        private DispatcherTimer timer = null;
        
        public void addToPlayList(string id)
        {
            MusicDetil mD = new MusicDetil(id);
            PlayList.Add(new MusicItem(mD.getName(), mD.GetMusicPicUrl(), mD.GetMusicUrl(), mD.GetMusicArtistAndID()));
            if (!isPlaying) intelligentMusicPlayerCore();
        }

        public void addToPlayList(PlayList play,int id)
        {
            ThreadInterlizeMusic tim = new ThreadInterlizeMusic();
            int index = 0;
            foreach (string ids in play.PlayLists.Keys)
            {
                if (index.Equals(id)) addToPlayList(ids);
                else tim.add(ids);
                play.PlayLists.Remove(play.PlayLists[ids]);
                index++;
            }
            Console.Write("开始生成musicitem");
            tim.run();
        }


        #region 播放器顺序代码
        public void intelligentMusicPlayerCore()
        {
            if (isPlaying) mediaPlayer.Stop();
            MusicItem mI = PlayList[playIndex];
            cw.Source = new BitmapImage(new Uri(mI.PicUrl, UriKind.Absolute));
            StringBuilder stB = new StringBuilder();

            foreach (string nameArtist in mI.Artists.Keys)
            {
                stB.Append(nameArtist + @"\");
            }
            Title.Text = mI.Title + "\n" + stB;

            playMusicUrl(mI.MusicUrl);
            GC.Collect();
        }

        public void intelligentMusicMethod(bool forward)
        {
            switch (playingType)
            {
                case 0:
                    if (forward)
                    {
                        playIndex++;
                        if (playIndex.Equals(PlayList.Count)) playIndex = 0;
                        intelligentMusicPlayerCore();
                    }
                    else
                    {
                        if (!(playIndex <= 0)) playIndex = PlayList.Count - 1;
                        else playIndex--;
                        intelligentMusicPlayerCore();
                    }
                    break;
            }
        }
        #endregion

        public void playMusicUrl(string url)
        {
            try
            {
                mediaPlayer.Source = new Uri(url, UriKind.Absolute);
                if (!mediaPlayer.Source.Equals(null)) mediaPlayer.Play();
                else showMessage("这个音乐可能需要对应平台vip或者没有版权!", "Tips 错误");
            }
            catch { showMessage("这个音乐可能需要对应平台vip或者没有版权!", "Tips 错误"); }
        }

        public void playMusic(string id)
        {
            MusicDetil mD = new MusicDetil(id);
            mediaPlayer.Source = new Uri(mD.GetMusicUrl(), UriKind.Absolute);
            if (!mediaPlayer.Source.Equals(null)) mediaPlayer.Play();
            else showMessage("这个音乐可能需要对应平台vip或者没有版权!", "Tips 错误");
        }

        private void MediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            playIcon.Glyph = "\xE769";
            isPlaying = true;
            sliderPosition.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
            RamUtils.ramRollBack();
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            if(mediaPlayer.Position.ToString().Length > 10) Time.Text = mediaPlayer.Position.ToString().Remove(mediaPlayer.Position.ToString().IndexOf(".")).Remove(0, 3);
            if (!isThumbDraging) sliderPosition.Value = mediaPlayer.Position.TotalSeconds;
        }

        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            isPlaying = false;
            RamUtils.ramRollBack();
        }

        private void SliderPosition_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isThumbDraging = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(sliderPosition.Value);
        }

        #region 播放器控制事件
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying) { mediaPlayer.Pause(); isPlaying = false; playIcon.Glyph = "\xE768"; }
            else{ mediaPlayer.Play(); isPlaying = true; playIcon.Glyph = "\xE769"; }
            GC.Collect();
        }

        private void NextPlay_Click(object sender, RoutedEventArgs e)
        {
            intelligentMusicMethod(true);
        }

        private void BackPlay_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void SliderPosition_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e) => isThumbDraging = true;
        #endregion

        private int showingIndex = 0;public static PlayList selectPlayList { get; set; }

        private void Menu_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationView menu = (NavigationView)sender;
            //{debugCode} //MessageBox.Show(menu.MenuItems.IndexOf(menu.SelectedItem).ToString());
            int index = menu.MenuItems.IndexOf(menu.SelectedItem);
            switch (index)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    selectPlayList = playLists[index - 4];
                    if (showingIndex.Equals(-1)) shower.Refresh();
                    shower.Navigate(typeof(PlayListPage));
                    menu.Header = "歌单:" + selectPlayList.Title;
                    MainWindow.selectPlayList = null;
                    break;
            }
        }

        public void showMessage(string text, string title)
        {
            MessageBox.Show(text, title);
        }

        private List<PlayList> playLists;

        public void LoginThread()
        {
            Task.Run(() =>
            {
                AccountUtils aU = new AccountUtils("18511732011", "ljybjhsy070219");
                playLists = aU.GetMusicList();
                foreach(PlayList playList in playLists)
                {
                    Dispatcher.Invoke(new Action(delegate{
                               SymbolIcon s1 = new SymbolIcon
                               {
                                   Symbol = Symbol.MusicInfo
                               };
                               menu.MenuItems.Add(new NavigationViewItem() { Content = playList.Title, Icon = s1 });
                           }));
                }
            });
        }
    }

    public class MusicItem
    {
        public string Title;
        public string PicUrl;
        public string MusicUrl;
        public Dictionary<string, string> Artists;

        public MusicItem(string title,string picurl,string musicurl, Dictionary<string, string> artist) {this.Artists = artist; Title = title;PicUrl = picurl;MusicUrl = musicurl; }
    }

    public class ThreadInterlizeMusic
    {

        private List<string> id = new List<string>();
        private int threadCount = 5,nowindex = 0;
        private bool end = false;

        public ThreadInterlizeMusic() { }

        public void add(string id) => this.id.Add(id);

        public void stop() => end = true;

        public void run()
        {
            nowindex = 0;
            Task.Run(() =>
            {
                while (!end)
                {
                    if (id.Count - 1 >= 0)
                    {
                        if (threadCount > 0)
                        {
                            new Task(() =>
                            {
                                nowindex++;
                                MusicDetil mD = new MusicDetil(id[nowindex]);
                                Console.WriteLine(id[nowindex]);
                                MainWindow.PlayList.Add(new MusicItem(mD.getName(), mD.GetMusicPicUrl(), mD.GetMusicUrl(), mD.GetMusicArtistAndID()));
                                threadCount++;
                            }).Start();
                            Thread.Sleep(1000);
                            if (nowindex >= (id.Count - 1)) break;
                            Console.WriteLine(threadCount);
                            threadCount--;
                        }
                        else { }
                    }
                }
            });
        }
    }
}
