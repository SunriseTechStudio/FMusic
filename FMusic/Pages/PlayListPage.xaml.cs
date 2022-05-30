using FMusic.Util;
using FMusic.Util.NetMusicCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace FMusic.Pages
{
    public partial class PlayListPage : Page
    {
        public delegate void PlayServer(int Index,PlayList play);
        public static event PlayServer PlayMusicDefult;

        public delegate void PlayServer1(string id);
        public static event PlayServer1 PlayMusicAdd;

        private PlayList playList;

        public PlayListPage()
        {
            InitializeComponent();
            playList = MainWindow.selectPlayList;
            foreach (string name in playList.PlayLists.Values) All.Items.Add(name);
            introduce.Document.Blocks.Clear();
            introduce.AppendText(playList.introduce);
            CI.Source = new BitmapImage(new Uri(playList.CoverImage, UriKind.Absolute));
            count.Text = playList.playCount;
            member.Text = playList.Creator;
            RamUtils.ramRollBack();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            PlayMusicDefult(All.SelectedIndex, playList);
        }
    }
}
